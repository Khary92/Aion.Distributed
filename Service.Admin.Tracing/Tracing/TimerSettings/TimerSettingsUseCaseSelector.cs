using Service.Admin.Tracing.Tracing.TimerSettings.UseCase;

namespace Service.Admin.Tracing.Tracing.TimerSettings;

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