namespace Service.Server.Old.Services.Entities.Settings;

public interface ISettingsRequestsService
{
    Task<Domain.Entities.Settings> Get();
    Task<bool> IsSettingsExisting();
}