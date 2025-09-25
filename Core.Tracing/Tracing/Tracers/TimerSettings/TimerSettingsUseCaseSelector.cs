using Core.Server.Tracing.Tracing.Tracers.TimerSettings.UseCase;

namespace Core.Server.Tracing.Tracing.Tracers.TimerSettings;

public class TimerSettingsUseCaseSelector(
    IChangeSnapshotSaveIntervalTraceCollector changeSnapshotSaveIntervalTraceCollector,
    IChangeDocuTimerSaveIntervalTraceCollector changeDocuTimerSaveIntervalTraceCollector)
    : ITimerSettingsUseCaseSelector
{
    public IChangeSnapshotSaveIntervalTraceCollector ChangeSnapshotInterval => changeSnapshotSaveIntervalTraceCollector;

    public IChangeDocuTimerSaveIntervalTraceCollector ChangeDocuTimerInterval =>
        changeDocuTimerSaveIntervalTraceCollector;
}