using Grpc.Net.Client;
using Proto.Command.TimerSettings;

namespace Service.Admin.Web.Communication.Commands.TimerSettings;

public class TimerSettingsCommandSender : ITimerSettingsCommandSender
{
    private readonly TimerSettingsCommandProtoService.TimerSettingsCommandProtoServiceClient _client;

    public TimerSettingsCommandSender(string address)
    {
        var channel = GrpcChannel.ForAddress(address);
        _client = new TimerSettingsCommandProtoService.TimerSettingsCommandProtoServiceClient(channel);
    }

    public async Task<bool> Send(CreateTimerSettingsCommandProto command)
    {
        var response = await _client.CreateTimerSettingsAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(ChangeSnapshotSaveIntervalCommandProto command)
    {
        var response = await _client.ChangeSnapshotSaveIntervalAsync(command);
        return response.Success;
    }

    public async Task<bool> Send(ChangeDocuTimerSaveIntervalCommandProto command)
    {
        var response = await _client.ChangeDocuTimerSaveIntervalAsync(command);
        return response.Success;
    }
}