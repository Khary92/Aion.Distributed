using Service.Admin.Tracing.Tracing.TimerSettings.UseCase;

namespace Service.Admin.Tracing.Tracing.TimerSettings;

public interface ITimerSettingsUseCaseSelector
{
    ICreateTimerSettingsTraceCollector Create { get; }
    IChangeSnapshotSaveIntervalTraceCollector ChangeSnapshotInterval { get; }
    IChangeDocuTimerSaveIntervalTraceCollector ChangeDocuTimerInterval { get; }
}