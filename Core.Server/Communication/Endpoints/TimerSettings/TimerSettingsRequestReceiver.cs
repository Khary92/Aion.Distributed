using Core.Server.Services.Entities.TimerSettings;
using Grpc.Core;
using Proto.DTO.TimerSettings;
using Proto.Requests.TimerSettings;

namespace Core.Server.Communication.Endpoints.TimerSettings;

public class TimerSettingsRequestReceiver(ITimerSettingsRequestsService timerSettingsRequestsService)
    : TimerSettingsRequestService.TimerSettingsRequestServiceBase
{
    public override async Task<TimerSettingsProto> GetTimerSettings(GetTimerSettingsRequestProto request,
        ServerCallContext context)
    {
        var timerSettings = await timerSettingsRequestsService.Get();
        return timerSettings.ToProto();
    }

    public override async Task<TimerSettingExistsProto> IsTimerSettingExisting(
        IsTimerSettingExistingRequestProto request,
        ServerCallContext context)
    {
        return new TimerSettingExistsProto
        {
            Exists = await timerSettingsRequestsService.IsTimerSettingsExisting()
        };
    }
}