namespace Service.Admin.Tracing.Tracing.TimerSettings.UseCase;

public interface IChangeDocuTimerSaveIntervalTraceCollector
{
    Task StartUseCase(Type originClassType, Guid traceId);
    Task SendingCommand(Type originClassType, Guid traceId, object command);
    Task NotificationReceived(Type originClassType, Guid traceId, object notification);
    Task ChangesApplied(Type originClassType, Guid traceId);
}