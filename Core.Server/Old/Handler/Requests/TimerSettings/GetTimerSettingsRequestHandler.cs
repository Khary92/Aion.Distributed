using Application.Contract.CQRS.Requests.TimerSettings;
using Application.Contract.DTO;
using Application.Services.Entities.TimerSettings;
using MediatR;

namespace Application.Handler.Requests.TimerSettings;

public class GetTimerSettingsRequestHandler(
    ITimerSettingsRequestsService timerSettingsRequestsService) :
    IRequestHandler<GetTimerSettingsRequest, TimerSettingsDto?>
{
    public async Task<TimerSettingsDto?> Handle(GetTimerSettingsRequest request, CancellationToken cancellationToken)
    {
        return await timerSettingsRequestsService.Get();
    }
}