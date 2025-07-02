using Core.Server.Tracing;
using Core.Server.Tracing.Communication.Tracing;
using Grpc.Net.Client;
using Proto.Command.TraceData;

namespace Service.Monitoring.Shared.Tracing;

public class TracingDataCommandSender : ITracingDataCommandSender
{
    private readonly TraceDataCommandProtoService.TraceDataCommandProtoServiceClient _client;

    public TracingDataCommandSender()
    {
        var channel = GrpcChannel.ForAddress(TempConnectionStatic.MonitoringAddress);
        _client = new(channel);
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