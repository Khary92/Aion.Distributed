namespace Client.Desktop.Tracing.Tracing.Tracers.TimerSettings.UseCase;

public interface IChangeSnapshotSaveIntervalTraceCollector
{
    Task StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes);
    Task CommandSent(Type originClassType, Guid traceId, object command);
    Task NotificationReceived(Type originClassType, Guid traceId, object notification);
    Task NoAggregateFound(Type originClassType, Guid traceId);
    Task ChangesApplied(Type originClassType, Guid traceId);
    Task PropertyNotChanged(Type originClassType, Guid traceId, Dictionary<string, string> asTraceAttributes);
    
}