using Grpc.Net.Client;
using Proto.Command.TraceData;
using Service.Monitoring.Shared;

namespace Core.Server.Tracing.Communication.Tracing;

public class TracingDataCommandSender : ITracingDataCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.MonitoringAddress, new GrpcChannelOptions
    {
        HttpHandler = new SocketsHttpHandler
        {
            EnableMultipleHttp2Connections = true,
            KeepAlivePingDelay = TimeSpan.FromSeconds(60),
            KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
            ConnectTimeout = TimeSpan.FromSeconds(30)
        }
    });

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