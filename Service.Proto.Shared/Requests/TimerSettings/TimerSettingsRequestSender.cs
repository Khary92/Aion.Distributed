using Grpc.Net.Client;
using Proto.DTO.TimerSettings;
using Proto.Requests.TimerSettings;

namespace Service.Proto.Shared.Requests.TimerSettings;

public class TimerSettingsRequestSender : ITimerSettingsRequestSender
{
    private readonly TimerSettingsProtoRequestService.TimerSettingsProtoRequestServiceClient _client;
    
    public TimerSettingsRequestSender(string address)
    {
        var channel = GrpcChannel.ForAddress(address);
        _client = new(channel);
    }
    
    public async Task<TimerSettingsProto> Send(GetTimerSettingsRequestProto request)
    {
        return await _client.GetTimerSettingsAsync(request);
    }

    public async Task<bool> Send(IsTimerSettingExistingRequestProto request)
    {
        var response = await _client.IsTimerSettingExistingAsync(request);
        return response.Exists;
    }
}