using Application.Contract.CQRS.Commands.Entities.TimerSettings;
using Application.Services.Entities.TimerSettings;
using MediatR;

namespace Application.Handler.Commands.Entities.TimerSettings;

public class CreateTimerSettingsCommandHandler(ITimerSettingsCommandsService timerSettingsCommandsService) :
    IRequestHandler<CreateTimerSettingsCommand, Unit>
{
    public async Task<Unit> Handle(CreateTimerSettingsCommand command, CancellationToken cancellationToken)
    {
        await timerSettingsCommandsService.Create(command);
        return Unit.Value;
    }
}