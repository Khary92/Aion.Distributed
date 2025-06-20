namespace Service.Server.Services.Entities.AiSettings;

public interface IAiSettingsRequestsService
{
    Task<Domain.Entities.AiSettings> Get();
    Task<bool> IsAiSettingsExisting();
}