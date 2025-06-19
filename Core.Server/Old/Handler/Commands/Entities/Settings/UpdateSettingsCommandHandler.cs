using Service.Server.CQRS.Commands.Entities.Settings;
using Service.Server.Old.Services.Entities.Settings;

namespace Service.Server.Old.Handler.Commands.Entities.Settings;

public class UpdateSettingsCommandHandler(ISettingsCommandsService settingsCommandsService)
    : IRequestHandler<UpdateSettingsCommand, Unit>
{
    public async Task<Unit> Handle(UpdateSettingsCommand command, CancellationToken cancellationToken)
    {
        await settingsCommandsService.Update(command);
        return Unit.Value;
    }
}