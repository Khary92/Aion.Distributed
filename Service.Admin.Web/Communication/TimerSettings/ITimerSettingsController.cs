using Service.Admin.Web.Services;

namespace Service.Admin.Web.Communication.TimerSettings;

public interface ITimerSettingsController
{
    InitializationType Type { get; }
    Task SaveSettingsAsync();
    Task InitializeComponents();
}