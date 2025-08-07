using Service.Admin.Tracing;
using Service.Admin.Web.Communication.TimerSettings.Records;
using Service.Admin.Web.Communication.TimerSettings.State;
using Service.Admin.Web.Services;

namespace Service.Admin.Web.Communication.TimerSettings;

public class TimerSettingsController(
    ITimerSettingsStateService timerSettingsStateService,
    ISharedCommandSender commandSender,
    ITraceCollector tracer) : IInitializeAsync, ITimerSettingsController
{
    private int _previousSnapshotSaveInterval;
    private int _previousDocumentationSaveInterval;

    public async Task SaveSettingsAsync()
    {
        if (_previousSnapshotSaveInterval != timerSettingsStateService.TimerSettings.SnapshotSaveInterval)
        {
            var traceId = Guid.NewGuid();
            await tracer.TimerSettings.ChangeSnapshotInterval.StartUseCase(GetType(), traceId);

            var changeSnapshotIntervalCommand = new WebChangeSnapshotSaveIntervalCommand(
                timerSettingsStateService.TimerSettings.TimerSettingsId,
                timerSettingsStateService.TimerSettings.SnapshotSaveInterval, traceId);

            await tracer.TimerSettings.ChangeSnapshotInterval.SendingCommand(GetType(), traceId,
                changeSnapshotIntervalCommand);
            await commandSender.Send(changeSnapshotIntervalCommand.ToProto());

            _previousSnapshotSaveInterval = timerSettingsStateService.TimerSettings.SnapshotSaveInterval;
        }

        if (_previousDocumentationSaveInterval != timerSettingsStateService.TimerSettings.DocumentationSaveInterval)
        {
            var traceId = Guid.NewGuid();

            await tracer.TimerSettings.ChangeDocuTimerInterval.StartUseCase(GetType(), traceId);

            var changeDocuIntervalCommand = new WebChangeDocuTimerSaveIntervalCommand(
                timerSettingsStateService.TimerSettings.TimerSettingsId,
                timerSettingsStateService.TimerSettings.DocumentationSaveInterval, traceId);

            await tracer.TimerSettings.ChangeDocuTimerInterval.SendingCommand(GetType(), traceId,
                changeDocuIntervalCommand);
            await commandSender.Send(changeDocuIntervalCommand.ToProto());

            _previousDocumentationSaveInterval = timerSettingsStateService.TimerSettings.DocumentationSaveInterval;
        }
    }

    public InitializationType Type => InitializationType.Controller;

    public Task InitializeComponents()
    {
        _previousSnapshotSaveInterval = timerSettingsStateService.TimerSettings.SnapshotSaveInterval;
        _previousDocumentationSaveInterval = timerSettingsStateService.TimerSettings.DocumentationSaveInterval;
        return Task.CompletedTask;
    }
}