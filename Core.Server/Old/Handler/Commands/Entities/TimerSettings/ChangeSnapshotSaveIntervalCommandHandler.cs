using Application.Contract.CQRS.Commands.Entities.TimerSettings;
using Application.Services.Entities.TimerSettings;
using MediatR;

namespace Application.Handler.Commands.Entities.TimerSettings;

public class ChangeSnapshotSaveIntervalCommandHandler(ITimerSettingsCommandsService timerSettingsCommandsService)
    : IRequestHandler<ChangeSnapshotSaveIntervalCommand, Unit>
{
    public async Task<Unit> Handle(ChangeSnapshotSaveIntervalCommand command, CancellationToken cancellationToken)
    {
        await timerSettingsCommandsService.ChangeSnapshotInterval(command);
        return Unit.Value;
    }
}