namespace Service.Admin.Tracing.Tracing.TimerSettings.UseCase;

public interface IChangeSnapshotSaveIntervalTraceCollector
{
    Task StartUseCase(Type originClassType, Guid traceId);
    Task SendingCommand(Type originClassType, Guid traceId, object command);
    Task NotificationReceived(Type originClassType, Guid traceId, object notification);
    Task NoAggregateFound(Type originClassType, Guid traceId);
    Task ChangesApplied(Type originClassType, Guid traceId);
    Task PropertyNotChanged(Type originClassType, Guid traceId, string asTraceAttributes);
}