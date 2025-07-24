using Proto.Command.TimerSettings;
using Proto.Requests.TimerSettings;
using Service.Admin.Web.Communication.TimerSettings.Notifications.TimerSettings;
using Service.Admin.Web.DTO;

namespace Service.Admin.Web.Communication.TimerSettings.State;

public class TimerSettingsStateService(ISharedRequestSender requestSender, ISharedCommandSender commandSender)
    : ITimerSettingsStateService
{
    public TimerSettingsDto TimerSettings { get; internal set; } = new(Guid.Empty, 0, 0);

    public event Action? OnStateChanged;

    public Task SetTimerSettings(TimerSettingsDto timerSettings)
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
            return;
        }

        await commandSender.Send(new CreateTimerSettingsCommandProto()
        {
            TimerSettingsId = Guid.NewGuid().ToString(),
            DocumentationSaveInterval = 30,
            SnapshotSaveInterval = 30
        });
    }

    public void Apply(WebDocuIntervalChangedNotification notification)
    {
        TimerSettings.DocumentationSaveInterval = notification.DocumentationSaveInterval;
    }

    public void Apply(WebSnapshotIntervalChangedNotification notification)
    {
        TimerSettings.SnapshotSaveInterval = notification.SnapshotSaveInterval;
    }
    
    private void NotifyStateChanged() => OnStateChanged?.Invoke();
}