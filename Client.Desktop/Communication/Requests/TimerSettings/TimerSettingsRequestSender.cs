using System;
using System.Threading.Tasks;
using Client.Desktop.DTO;
using Grpc.Net.Client;
using Proto.Requests.TimerSettings;
using Proto.Shared;

namespace Client.Desktop.Communication.Requests.TimerSettings;

public class TimerSettingsRequestSender : ITimerSettingsRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.ServerAddress);
    private readonly TimerSettingsRequestService.TimerSettingsRequestServiceClient _client = new(Channel);

    public async Task<TimerSettingsDto> Send(GetTimerSettingsRequestProto request)
    {
        var response = await _client.GetTimerSettingsAsync(request);
        return new TimerSettingsDto(Guid.Parse(response.TimerSettingsId), response.DocumentationSaveInterval,
            response.SnapshotSaveInterval);
    }

    public async Task<bool> Send(IsTimerSettingExistingRequestProto request)
    {
        var response = await _client.IsTimerSettingExistingAsync(request);
        return response.Exists;
    }
}