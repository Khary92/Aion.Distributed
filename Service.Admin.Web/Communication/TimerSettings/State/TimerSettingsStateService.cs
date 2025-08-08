using Proto.Requests.TimerSettings;
using Service.Admin.Tracing;
using Service.Admin.Web.Communication.TimerSettings.Notifications;
using Service.Admin.Web.Communication.TimerSettings.Records;
using Service.Admin.Web.Communication.Wrappers;
using Service.Admin.Web.Models;
using Service.Admin.Web.Services;

namespace Service.Admin.Web.Communication.TimerSettings.State;

public class TimerSettingsStateService(
    ISharedRequestSender requestSender,
    ISharedCommandSender commandSender,
    ITraceCollector tracer)
    : ITimerSettingsStateService, IInitializeAsync
{
    public TimerSettingsWebModel TimerSettings { get; private set; } = new(Guid.Empty, 0, 0);

    public event Action? OnStateChanged;

    public async Task SetTimerSettings(NewTimerSettingsMessage timerSettingsMessage)
    {
        TimerSettings = timerSettingsMessage.TimerSettings;
        await tracer.TimerSettings.Create.AggregateAdded(GetType(), timerSettingsMessage.TraceId);
        NotifyStateChanged();
    }

    public async Task Apply(WebDocuIntervalChangedNotification notification)
    {
        TimerSettings.DocumentationSaveInterval = notification.DocumentationSaveInterval;
        await tracer.TimerSettings.ChangeDocuTimerInterval.ChangesApplied(GetType(), notification.TraceId);
        NotifyStateChanged();
    }

    public async Task Apply(WebSnapshotIntervalChangedNotification notification)
    {
        TimerSettings.SnapshotSaveInterval = notification.SnapshotSaveInterval;
        await tracer.TimerSettings.ChangeSnapshotInterval.ChangesApplied(GetType(), notification.TraceId);
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
            TimerSettings = timerSettingsProto.ToWebModel();
            NotifyStateChanged();
            return;
        }

        var traceId = Guid.NewGuid();
        await tracer.TimerSettings.Create.StartUseCase(GetType(), traceId);

        var webCreateTimerSettingsCommand = new WebCreateTimerSettingsCommand(Guid.NewGuid(), 30, 30, traceId);
        await tracer.TimerSettings.Create.SendingCommand(GetType(), traceId, webCreateTimerSettingsCommand);
        await commandSender.Send(webCreateTimerSettingsCommand.ToProto());
    }
}