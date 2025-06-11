using System.Threading.Tasks;
using Client.Avalonia.Communication.Commands;
using Grpc.Net.Client;
using Proto.Requests.TimerSettings;

namespace Client.Avalonia.Communication.Requests.TimerSettings;

public class TimerSettingsRequestSender : ITimerSettingsRequestSender
{
    private static readonly GrpcChannel Channel = GrpcChannel.ForAddress(TempConnectionStatic.Address);
    private readonly TimerSettingsRequestService.TimerSettingsRequestServiceClient _client = new(Channel);

    public async Task<TimerSettingsProto> GetTimerSettings()
    {
        var request = new GetTimerSettingsRequestProto();
        var response = await _client.GetTimerSettingsAsync(request);
        return response;
    }

    public async Task<bool> IsTimerSettingExisting()
    {
        var request = new IsTimerSettingExistingRequestProto();
        var response = await _client.IsTimerSettingExistingAsync(request);
        return response.Exists;
    }
}