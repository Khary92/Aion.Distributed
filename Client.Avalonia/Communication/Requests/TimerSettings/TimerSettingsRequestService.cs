using System;
using System.Threading.Tasks;
using Contract.DTO;
using Grpc.Net.Client;
using Proto.Requests.TimerSettings;
using Proto.Shared;

namespace Client.Avalonia.Communication.Requests.TimerSettings;

public class TimerSettingsRequestSender : ITimerSettingsRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.Address);
    private readonly TimerSettingsRequestService.TimerSettingsRequestServiceClient _client = new(Channel);

    public async Task<TimerSettingsDto> GetTimerSettings()
    {
        var request = new GetTimerSettingsRequestProto();
        var response = await _client.GetTimerSettingsAsync(request);
        return new TimerSettingsDto(Guid.Parse(response.TimerSettingsId), response.DocumentationSaveInterval,
            response.SnapshotSaveInterval);
    }

    public async Task<bool> IsTimerSettingExisting()
    {
        var request = new IsTimerSettingExistingRequestProto();
        var response = await _client.IsTimerSettingExistingAsync(request);
        return response.Exists;
    }
}