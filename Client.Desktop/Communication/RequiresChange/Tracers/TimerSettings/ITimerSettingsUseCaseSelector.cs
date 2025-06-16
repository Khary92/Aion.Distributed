using Client.Desktop.Communication.RequiresChange.Tracers.TimerSettings.UseCase;

namespace Client.Desktop.Communication.RequiresChange.Tracers.TimerSettings;

public interface ITimerSettingsUseCaseSelector
{
    ICreateTimerSettingsTraceCollector Create { get; }
    IChangeSnapshotSaveIntervalTraceCollector ChangeSnapshotInterval { get; }
    IChangeDocuTimerSaveIntervalTraceCollector ChangeDocuTimerInterval { get; }
}