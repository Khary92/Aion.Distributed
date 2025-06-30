using Grpc.Net.Client;
using Proto.Command.TraceData;
using Service.Monitoring.Shared;

namespace Core.Server.Tracing.Communication.Tracing;

public class TracingDataCommandSender : ITracingDataCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.MonitoringAddress);
    private readonly TraceDataCommandProtoService.TraceDataCommandProtoServiceClient _client = new(Channel);

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