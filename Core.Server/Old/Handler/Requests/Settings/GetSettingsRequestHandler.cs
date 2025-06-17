using Application.Contract.CQRS.Commands.Entities.Settings;
using Application.Contract.CQRS.Requests.Settings;
using Application.Contract.DTO;
using Application.Services.Entities.Settings;
using MediatR;

namespace Application.Handler.Requests.Settings;

public class GetSettingsRequestHandler(
    IMediator mediator,
    ISettingsRequestsService settingsRequestsService) :
    IRequestHandler<GetSettingsRequest, SettingsDto?>
{
    public async Task<SettingsDto?> Handle(GetSettingsRequest request, CancellationToken cancellationToken)
    {
        var settingsDto = await settingsRequestsService.Get();

        if (settingsDto != null) return settingsDto;

        await mediator.Send(new CreateSettingsCommand(Guid.NewGuid(), string.Empty, false), cancellationToken);
        
        return new SettingsDto(Guid.NewGuid(), string.Empty, false);
    }
}