using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Command.TraceReports;
using Proto.Shared;

namespace Client.Desktop.Communication.Commands.TraceReports;

public class TraceReportCommandSender : ITraceReportCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.Address);
    private readonly TraceReportCommandService.TraceReportCommandServiceClient _client = new(Channel);

    public async Task<bool> Send(SendTraceReportCommand command)
    {
        var response = await _client.SendTraceReportAsync(command);
        return response.Success;
    }
}