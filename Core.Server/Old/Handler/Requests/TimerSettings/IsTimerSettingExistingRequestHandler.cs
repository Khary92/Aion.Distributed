using Service.Server.CQRS.Requests.TimerSettings;
using Service.Server.Old.Services.Entities.TimerSettings;

namespace Service.Server.Old.Handler.Requests.TimerSettings;

public class IsTimerSettingExistingRequestHandler(
    ITimerSettingsRequestsService timerSettingsRequestsService) :
    IRequestHandler<IsTimerSettingExistingRequest, bool>
{
    public async Task<bool> Handle(IsTimerSettingExistingRequest request, CancellationToken cancellationToken)
    {
        return await timerSettingsRequestsService.Get() != null;
    }
}