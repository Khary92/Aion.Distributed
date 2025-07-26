using Grpc.Net.Client;
using Proto.Command.TraceData;

namespace Service.Monitoring.Shared.Tracing;

public class TracingDataCommandSender : ITracingDataCommandSender
{
    private readonly TraceDataCommandProtoService.TraceDataCommandProtoServiceClient _client;

    public TracingDataCommandSender(string address)
    {
        var channel = GrpcChannel.ForAddress(address);
        _client = new TraceDataCommandProtoService.TraceDataCommandProtoServiceClient(channel);
    }


    public async Task<bool> Send(ServiceTraceDataCommand command)
    {
        try
        {
            var response = await _client.SendTraceDataAsync(command.ToProto());
            return response.Success;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}