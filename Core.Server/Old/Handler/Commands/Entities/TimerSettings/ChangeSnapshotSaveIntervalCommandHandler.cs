using Service.Server.CQRS.Commands.Entities.TimerSettings;
using Service.Server.Old.Services.Entities.TimerSettings;

namespace Service.Server.Old.Handler.Commands.Entities.TimerSettings;

public class ChangeSnapshotSaveIntervalCommandHandler(ITimerSettingsCommandsService timerSettingsCommandsService)
    : IRequestHandler<ChangeSnapshotSaveIntervalCommand, Unit>
{
    public async Task<Unit> Handle(ChangeSnapshotSaveIntervalCommand command, CancellationToken cancellationToken)
    {
        await timerSettingsCommandsService.ChangeSnapshotInterval(command);
        return Unit.Value;
    }
}