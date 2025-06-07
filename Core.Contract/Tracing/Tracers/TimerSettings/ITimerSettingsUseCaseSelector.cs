using Contract.Tracing.Tracers.TimerSettings.UseCase;

namespace Contract.Tracing.Tracers.TimerSettings;

public interface ITimerSettingsUseCaseSelector
{
    ICreateTimerSettingsTraceCollector Create { get; }
    IChangeSnapshotSaveIntervalTraceCollector ChangeSnapshotInterval { get; }
    IChangeDocuTimerSaveIntervalTraceCollector ChangeDocuTimerInterval { get; }
}