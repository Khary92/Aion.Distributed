using Application.Contract.CQRS.Commands.Entities.AiSettings;
using Application.Contract.CQRS.Requests.AiSettings;
using Application.Contract.DTO;
using Application.Services.Entities.AiSettings;
using MediatR;

namespace Application.Handler.Requests.AiSettings;

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