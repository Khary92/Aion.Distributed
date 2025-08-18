namespace Client.Tracing.Tracing.Tracers.Tag.UseCase;

public interface IUpdateTagTraceCollector
{
    Task NotificationReceived(Type originClassType, Guid traceId, object notification);
    Task NoAggregateFound(Type originClassType, Guid traceId);
    Task ChangesApplied(Type originClassType, Guid traceId);
}