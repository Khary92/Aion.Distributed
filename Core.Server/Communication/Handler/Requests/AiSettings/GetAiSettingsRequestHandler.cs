using Service.Server.CQRS.Commands.Entities.AiSettings;
using Service.Server.CQRS.Requests.AiSettings;
using Service.Server.Old.Services.Entities.AiSettings;

namespace Service.Server.Old.Handler.Requests.AiSettings;

public class GetAiSettingsRequestHandler(
    IAiSettingsRequestsService aiSettingsRequestsService,
    IMediator mediator) :
    IRequestHandler<GetAiSettingsRequest, AiSettingsDto>
{
    public async Task<AiSettingsDto> Handle(GetAiSettingsRequest request, CancellationToken cancellationToken)
    {
        var aiSettingsDto = await aiSettingsRequestsService.Get();

        if (aiSettingsDto != null) return aiSettingsDto;

        var aiSettingsId = Guid.NewGuid();
        var newAiSettingsDto = new AiSettingsDto(aiSettingsId, string.Empty, string.Empty);

        await mediator.Send(new CreateAiSettingsCommand(aiSettingsId, string.Empty, string.Empty),
            cancellationToken);

        return newAiSettingsDto;
    }
}