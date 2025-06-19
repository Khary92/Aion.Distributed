namespace Service.Server.Old.Services.Entities.AiSettings;

public interface IAiSettingsRequestsService
{
    Task<AiSettingsDto?> Get();
}