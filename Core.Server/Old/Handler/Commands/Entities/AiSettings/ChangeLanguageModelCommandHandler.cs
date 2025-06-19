using Service.Server.CQRS.Commands.Entities.AiSettings;
using Service.Server.Old.Services.Entities.AiSettings;

namespace Service.Server.Old.Handler.Commands.Entities.AiSettings;

public class ChangeLanguageModelCommandHandler(IAiSettingsCommandsService aiSettingsCommandsService)
    : IRequestHandler<ChangeLanguageModelCommand, Unit>
{
    public async Task<Unit> Handle(ChangeLanguageModelCommand request, CancellationToken cancellationToken)
    {
        await aiSettingsCommandsService.ChangeLanguageModelPath(request);
        return Unit.Value;
    }
}