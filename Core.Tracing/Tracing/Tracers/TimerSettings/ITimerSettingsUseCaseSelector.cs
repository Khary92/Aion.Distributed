using Core.Server.Tracing.Tracing.Tracers.TimerSettings.UseCase;

namespace Core.Server.Tracing.Tracing.Tracers.TimerSettings;

public interface ITimerSettingsUseCaseSelector
{
    ICreateTimerSettingsTraceCollector Create { get; }
    IChangeSnapshotSaveIntervalTraceCollector ChangeSnapshotInterval { get; }
    IChangeDocuTimerSaveIntervalTraceCollector ChangeDocuTimerInterval { get; }
}