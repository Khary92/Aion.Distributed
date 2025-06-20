namespace Service.Server.Old.Services.Entities.TimerSettings;

public interface ITimerSettingsRequestsService
{
    Task<Domain.Entities.TimerSettings> Get();
    Task<bool> IsTimerSettingsExisting();
}