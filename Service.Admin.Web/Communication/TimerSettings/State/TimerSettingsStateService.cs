using Proto.Command.TimerSettings;
using Proto.DTO.TraceData;
using Proto.Requests.TimerSettings;
using Service.Admin.Web.Communication.TimerSettings.Notifications;
using Service.Admin.Web.Models;
using Service.Admin.Web.Services;

namespace Service.Admin.Web.Communication.TimerSettings.State;

public class TimerSettingsStateService(ISharedRequestSender requestSender, ISharedCommandSender commandSender)
    : ITimerSettingsStateService, IInitializeAsync
{
    public TimerSettingsWebModel TimerSettings { get; private set; } = new(Guid.Empty, 0, 0);

    public event Action? OnStateChanged;

    public Task SetTimerSettings(TimerSettingsWebModel timerSettings)
    {
        TimerSettings = timerSettings;
        return Task.CompletedTask;
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

    public InitializationType Type => InitializationType.StateService;
    public async Task InitializeComponents()
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
            SnapshotSaveInterval = 30,
            TraceData = new TraceDataProto()
            {
                TraceId = Guid.NewGuid().ToString()
            }
        });
    }
}