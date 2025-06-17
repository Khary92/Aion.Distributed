using Application.Contract.CQRS.Requests.TimerSettings;
using Application.Services.Entities.TimerSettings;
using MediatR;

namespace Application.Handler.Requests.TimerSettings;

public class IsTimerSettingExistingRequestHandler(
    ITimerSettingsRequestsService timerSettingsRequestsService) :
    IRequestHandler<IsTimerSettingExistingRequest, bool>
{
    public async Task<bool> Handle(IsTimerSettingExistingRequest request, CancellationToken cancellationToken)
    {
        return await timerSettingsRequestsService.Get() != null;
    }
}