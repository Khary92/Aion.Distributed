namespace Service.Server.Old.Services.Entities.TimerSettings;

public interface ITimerSettingsRequestsService
{
    Task<TimerSettingsDto?> Get();
}