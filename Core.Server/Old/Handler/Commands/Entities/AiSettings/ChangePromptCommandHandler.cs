using Service.Server.CQRS.Commands.Entities.AiSettings;
using Service.Server.Old.Services.Entities.AiSettings;

namespace Service.Server.Old.Handler.Commands.Entities.AiSettings;

public class ChangePromptCommandHandler(IAiSettingsCommandsService aiSettingsCommandsService)
    : IRequestHandler<ChangePromptCommand, Unit>
{
    public async Task<Unit> Handle(ChangePromptCommand request, CancellationToken cancellationToken)
    {
        await aiSettingsCommandsService.ChangePrompt(request);
        return Unit.Value;
    }
}