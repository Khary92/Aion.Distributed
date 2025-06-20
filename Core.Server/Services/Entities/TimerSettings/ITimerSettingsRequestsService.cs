namespace Core.Server.Services.Entities.TimerSettings;

public interface ITimerSettingsRequestsService
{
    Task<Domain.Entities.TimerSettings> Get();
    Task<bool> IsTimerSettingsExisting();
}