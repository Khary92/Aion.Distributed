using Service.Server.CQRS.Requests.TimerSettings;
using Service.Server.Old.Services.Entities.TimerSettings;

namespace Service.Server.Old.Handler.Requests.TimerSettings;

public class GetTimerSettingsRequestHandler(
    ITimerSettingsRequestsService timerSettingsRequestsService) :
    IRequestHandler<GetTimerSettingsRequest, TimerSettingsDto?>
{
    public async Task<TimerSettingsDto?> Handle(GetTimerSettingsRequest request, CancellationToken cancellationToken)
    {
        return await timerSettingsRequestsService.Get();
    }
}