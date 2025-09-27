using Proto.Requests.TimerSettings;
using Service.Admin.Tracing;
using Service.Admin.Web.Communication.Mapper;
using Service.Admin.Web.Communication.Records.Notifications;
using Service.Admin.Web.Communication.Sender.Common;
using Service.Admin.Web.Models;
using Service.Admin.Web.Services.Startup;

namespace Service.Admin.Web.Services.State;

public class TimerSettingsStateService(
    ISharedRequestSender requestSender,
    ITraceCollector tracer)
    : ITimerSettingsStateService, IInitializeAsync
{
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

        throw new Exception("Timer settings not found");
    }

    public TimerSettingsWebModel TimerSettings { get; private set; } = new(Guid.Empty, 0, 0);

    public event Action? OnStateChanged;
    
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
}