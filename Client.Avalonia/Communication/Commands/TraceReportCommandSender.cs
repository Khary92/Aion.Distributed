using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Command.TraceReports;

namespace Client.Avalonia.Communication.Sender;

public class TraceReportCommandSender : ITraceReportCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempStatic.Address);
    private readonly TraceReportCommandService.TraceReportCommandServiceClient _client = new(Channel);

    public async Task<bool> Send(SendTraceReportCommand command)
    {
        var response = await _client.SendTraceReportAsync(command);
        return response.Success;
    }
}