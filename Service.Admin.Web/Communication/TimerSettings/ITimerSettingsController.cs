using Service.Admin.Web.Services;

namespace Service.Admin.Web.Communication.TimerSettings;

public interface ITimerSettingsController
{
    Task SaveSettingsAsync();
    InitializationType Type { get; }
    Task InitializeComponents();
}