using Grpc.Core;
using Grpc.Net.Client;
using Proto.Command.TraceData;
using Service.Admin.Tracing.Tracing;
using Service.Admin.Web.Communication.Authentication;
using Service.Monitoring.Shared;

namespace Service.Admin.Web.Communication.Tracing;

public class TracingDataSender : ITracingDataSender
{
    private readonly JwtService _jwtService;
    private readonly TraceDataCommandProtoService.TraceDataCommandProtoServiceClient _client;

    public TracingDataSender(string address, JwtService jwtService)
    {
        _jwtService = jwtService;
        var channel = GrpcChannel.ForAddress(address);
        _client = new TraceDataCommandProtoService.TraceDataCommandProtoServiceClient(channel);
    }
    
    private Metadata GetAuthHeader() => new()
    {
        { "Authorization", $"Bearer {_jwtService.Token}" }
    };

    public async Task<bool> Send(ServiceTraceDataCommand command)
    {
        try
        {
            var response = await _client.SendTraceDataAsync(command.ToProto(), GetAuthHeader());
            return response.Success;
        }
        catch (Exception ex)
        {
            Console.WriteLine(GetType() + " caused an error: " + ex);
            return false;
        }
    }
}