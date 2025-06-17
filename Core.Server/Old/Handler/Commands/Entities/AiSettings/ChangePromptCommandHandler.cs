using Application.Contract.CQRS.Commands.Entities.AiSettings;
using Application.Services.Entities.AiSettings;
using MediatR;

namespace Application.Handler.Commands.Entities.AiSettings;

public class ChangePromptCommandHandler(IAiSettingsCommandsService aiSettingsCommandsService)
    : IRequestHandler<ChangePromptCommand, Unit>
{
    public async Task<Unit> Handle(ChangePromptCommand request, CancellationToken cancellationToken)
    {
        await aiSettingsCommandsService.ChangePrompt(request);
        return Unit.Value;
    }
}