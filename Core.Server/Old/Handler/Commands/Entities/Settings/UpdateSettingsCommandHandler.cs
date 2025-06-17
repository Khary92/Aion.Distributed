using Application.Contract.CQRS.Commands.Entities.Settings;
using Application.Services.Entities.Settings;
using MediatR;

namespace Application.Handler.Commands.Entities.Settings;

public class UpdateSettingsCommandHandler(ISettingsCommandsService settingsCommandsService)
    : IRequestHandler<UpdateSettingsCommand, Unit>
{
    public async Task<Unit> Handle(UpdateSettingsCommand command, CancellationToken cancellationToken)
    {
        await settingsCommandsService.Update(command);
        return Unit.Value;
    }
}