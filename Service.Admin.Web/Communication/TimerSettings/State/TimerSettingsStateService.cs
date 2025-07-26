using Proto.Command.TimerSettings;
using Proto.Requests.TimerSettings;
using Service.Admin.Web.Communication.TimerSettings.Notifications;
using Service.Admin.Web.Models;
using Service.Admin.Web.Services;

namespace Service.Admin.Web.Communication.TimerSettings.State;

public class TimerSettingsStateService(ISharedRequestSender requestSender, ISharedCommandSender commandSender)
    : ITimerSettingsStateService
{

    public TimerSettingsWebModel TimerSettings { get; internal set; } = new(Guid.Empty, 0, 0);

    public event Action? OnStateChanged;

    public Task SetTimerSettings(TimerSettingsWebModel timerSettings)
    {
        TimerSettings = timerSettings;
        return Task.CompletedTask;
    }

    public async Task LoadSettings()
    {
        if (await requestSender.Send(new IsTimerSettingExistingRequestProto()))
        {
            var timerSettingsProto = await requestSender.Send(new GetTimerSettingsRequestProto());
            TimerSettings = timerSettingsProto.ToDto();
            NotifyStateChanged();
            return;
        }

        await commandSender.Send(new CreateTimerSettingsCommandProto
        {
            TimerSettingsId = Guid.NewGuid().ToString(),
            DocumentationSaveInterval = 30,
            SnapshotSaveInterval = 30
        });
    }

    public void Apply(WebDocuIntervalChangedNotification notification)
    {
        TimerSettings.DocumentationSaveInterval = notification.DocumentationSaveInterval;
        NotifyStateChanged();
    }

    public void Apply(WebSnapshotIntervalChangedNotification notification)
    {
        TimerSettings.SnapshotSaveInterval = notification.SnapshotSaveInterval;
        NotifyStateChanged();
    }
    
    private void NotifyStateChanged()
    {
        OnStateChanged?.Invoke();
    }
}