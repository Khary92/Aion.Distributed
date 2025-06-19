using Service.Server.CQRS.Commands.Entities.AiSettings;
using Service.Server.Old.Services.Entities.AiSettings;

namespace Service.Server.Old.Handler.Commands.Entities.AiSettings;

public class CreateAiSettingsCommandHandler(IAiSettingsCommandsService configCommandsService)
    : IRequestHandler<CreateAiSettingsCommand, Unit>
{
    public async Task<Unit> Handle(CreateAiSettingsCommand request, CancellationToken cancellationToken)
    {
        await configCommandsService.Create(request);
        return Unit.Value;
    }
}