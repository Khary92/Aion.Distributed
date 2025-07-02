using Client.Tracing.Tracing.Tracers.TimerSettings.UseCase;

namespace Client.Tracing.Tracing.Tracers.TimerSettings;

public interface ITimerSettingsUseCaseSelector
{
    ICreateTimerSettingsTraceCollector Create { get; }
    IChangeSnapshotSaveIntervalTraceCollector ChangeSnapshotInterval { get; }
    IChangeDocuTimerSaveIntervalTraceCollector ChangeDocuTimerInterval { get; }
}