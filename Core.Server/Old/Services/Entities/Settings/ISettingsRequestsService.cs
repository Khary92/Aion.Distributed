namespace Service.Server.Old.Services.Entities.Settings;

public interface ISettingsRequestsService
{
    Task<SettingsDto?> Get();
}