using Service.Admin.Tracing.Tracing.TimerSettings.UseCase;

namespace Service.Admin.Tracing.Tracing.TimerSettings;

public interface ITimerSettingsUseCaseSelector
{
    IChangeSnapshotSaveIntervalTraceCollector ChangeSnapshotInterval { get; }
    IChangeDocuTimerSaveIntervalTraceCollector ChangeDocuTimerInterval { get; }
}