using Application.Contract.CQRS.Commands.Entities.TimerSettings;
using Application.Services.Entities.TimerSettings;
using MediatR;

namespace Application.Handler.Commands.Entities.TimerSettings;

public class ChangeDocuTimerIntervalCommandHandler(ITimerSettingsCommandsService timerSettingsCommandsService)
    : IRequestHandler<ChangeDocuTimerSaveIntervalCommand, Unit>
{
    public async Task<Unit> Handle(ChangeDocuTimerSaveIntervalCommand command, CancellationToken cancellationToken)
    {
        await timerSettingsCommandsService.ChangeDocumentationInterval(command);
        return Unit.Value;
    }
}