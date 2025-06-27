using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Client;
using Proto.Command.TraceReports;

namespace Client.Desktop.Communication.Commands.TraceReports;

public class TraceReportCommandSender : ITraceReportCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly TraceReportCommandService.TraceReportCommandServiceClient _client = new(Channel);

    public async Task<bool> Send(SendTraceReportCommandProto command)
    {
        var response = await _client.SendTraceReportAsync(command);
        return response.Success;
    }
}