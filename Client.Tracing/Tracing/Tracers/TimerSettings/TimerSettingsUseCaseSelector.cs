using Client.Tracing.Tracing.Tracers.TimerSettings.UseCase;

namespace Client.Tracing.Tracing.Tracers.TimerSettings;

public class TimerSettingsUseCaseSelector(
    ICreateTimerSettingsTraceCollector timerSettingsTraceCollector,
    IChangeSnapshotSaveIntervalTraceCollector changeSnapshotSaveIntervalTraceCollector,
    IChangeDocuTimerSaveIntervalTraceCollector changeDocuTimerSaveIntervalTraceCollector)
    : ITimerSettingsUseCaseSelector
{
    public ICreateTimerSettingsTraceCollector Create => timerSettingsTraceCollector;
    public IChangeSnapshotSaveIntervalTraceCollector ChangeSnapshotInterval => changeSnapshotSaveIntervalTraceCollector;

    public IChangeDocuTimerSaveIntervalTraceCollector ChangeDocuTimerInterval =>
        changeDocuTimerSaveIntervalTraceCollector;
}