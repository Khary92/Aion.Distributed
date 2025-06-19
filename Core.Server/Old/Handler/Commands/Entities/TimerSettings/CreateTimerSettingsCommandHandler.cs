using Service.Server.CQRS.Commands.Entities.TimerSettings;
using Service.Server.Old.Services.Entities.TimerSettings;

namespace Service.Server.Old.Handler.Commands.Entities.TimerSettings;

public class CreateTimerSettingsCommandHandler(ITimerSettingsCommandsService timerSettingsCommandsService) :
    IRequestHandler<CreateTimerSettingsCommand, Unit>
{
    public async Task<Unit> Handle(CreateTimerSettingsCommand command, CancellationToken cancellationToken)
    {
        await timerSettingsCommandsService.Create(command);
        return Unit.Value;
    }
}