using System.Threading.Tasks;
using Grpc.Net.Client;
using Proto.Command.TimerSettings;
using Proto.Shared;

namespace Client.Desktop.Communication.Commands.TimerSettings;

public class TimerSettingsCommandSender : ITimerSettingsCommandSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.Address);
    private readonly TimerSettingsCommandService.TimerSettingsCommandServiceClient _client = new(Channel);

    public async Task<bool> Send(CreateTimerSettingsCommand createTicketCommand)
    {
        var response = await _client.CreateTimerSettingsAsync(createTicketCommand);
        return response.Success;
    }

    public async Task<bool> Send(ChangeSnapshotSaveIntervalCommand command)
    {
        var response = await _client.ChangeSnapshotSaveIntervalAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(ChangeDocuTimerSaveIntervalCommand command)
    {
        var response = await _client.ChangeDocuTimerSaveIntervalAsync(command);
        return response.Success;
    }
}