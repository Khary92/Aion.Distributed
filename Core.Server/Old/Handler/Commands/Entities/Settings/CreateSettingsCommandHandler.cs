using Application.Contract.CQRS.Commands.Entities.Settings;
using Application.Services.Entities.Settings;
using MediatR;

namespace Application.Handler.Commands.Entities.Settings;

public class CreateSettingsCommandHandler(ISettingsCommandsService settingsCommandsService) :
    IRequestHandler<CreateSettingsCommand, Unit>
{
    public async Task<Unit> Handle(CreateSettingsCommand command, CancellationToken cancellationToken)
    {
        await settingsCommandsService.Create(command);
        return Unit.Value;
    }
}