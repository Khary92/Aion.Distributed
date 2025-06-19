using Service.Server.CQRS.Commands.Entities.Settings;
using Service.Server.CQRS.Requests.Settings;
using Service.Server.Old.Services.Entities.Settings;

namespace Service.Server.Old.Handler.Requests.Settings;

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