using Application.Contract.CQRS.Commands.Entities.AiSettings;
using Application.Services.Entities.AiSettings;
using MediatR;

namespace Application.Handler.Commands.Entities.AiSettings;

public class ChangeLanguageModelCommandHandler(IAiSettingsCommandsService aiSettingsCommandsService)
    : IRequestHandler<ChangeLanguageModelCommand, Unit>
{
    public async Task<Unit> Handle(ChangeLanguageModelCommand request, CancellationToken cancellationToken)
    {
        await aiSettingsCommandsService.ChangeLanguageModelPath(request);
        return Unit.Value;
    }
}