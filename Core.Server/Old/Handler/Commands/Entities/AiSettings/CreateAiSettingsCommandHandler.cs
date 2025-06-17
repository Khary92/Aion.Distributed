using Application.Contract.CQRS.Commands.Entities.AiSettings;
using Application.Services.Entities.AiSettings;
using MediatR;

namespace Application.Handler.Commands.Entities.AiSettings;

public class CreateAiSettingsCommandHandler(IAiSettingsCommandsService configCommandsService)
    : IRequestHandler<CreateAiSettingsCommand, Unit>
{
    public async Task<Unit> Handle(CreateAiSettingsCommand request, CancellationToken cancellationToken)
    {
        await configCommandsService.Create(request);
        return Unit.Value;
    }
}