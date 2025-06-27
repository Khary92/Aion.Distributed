using Client.Desktop.Proto;
using Client.Desktop.Tracing.Tracing;
using Grpc.Net.Client;
using Proto.Command.TraceData;

namespace Client.Desktop.Tracing.Communication.Tracing;

public class TracingDataCommandSender : ITracingDataCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.MonitoringAddress);
    private readonly TraceDataCommandProtoService.TraceDataCommandProtoServiceClient _client = new(Channel);

    public async Task<bool> Send(TraceDataCommand command)
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