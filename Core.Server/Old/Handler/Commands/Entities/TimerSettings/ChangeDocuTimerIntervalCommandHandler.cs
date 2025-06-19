using Service.Server.CQRS.Commands.Entities.TimerSettings;
using Service.Server.Old.Services.Entities.TimerSettings;

namespace Service.Server.Old.Handler.Commands.Entities.TimerSettings;

public class ChangeDocuTimerIntervalCommandHandler(ITimerSettingsCommandsService timerSettingsCommandsService)
    : IRequestHandler<ChangeDocuTimerSaveIntervalCommand, Unit>
{
    public async Task<Unit> Handle(ChangeDocuTimerSaveIntervalCommand command, CancellationToken cancellationToken)
    {
        await timerSettingsCommandsService.ChangeDocumentationInterval(command);
        return Unit.Value;
    }
}