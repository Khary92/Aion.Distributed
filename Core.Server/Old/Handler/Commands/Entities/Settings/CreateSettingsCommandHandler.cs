using Service.Server.CQRS.Commands.Entities.Settings;
using Service.Server.Old.Services.Entities.Settings;

namespace Service.Server.Old.Handler.Commands.Entities.Settings;

public class CreateSettingsCommandHandler(ISettingsCommandsService settingsCommandsService) :
    IRequestHandler<CreateSettingsCommand, Unit>
{
    public async Task<Unit> Handle(CreateSettingsCommand command, CancellationToken cancellationToken)
    {
        await settingsCommandsService.Create(command);
        return Unit.Value;
    }
}