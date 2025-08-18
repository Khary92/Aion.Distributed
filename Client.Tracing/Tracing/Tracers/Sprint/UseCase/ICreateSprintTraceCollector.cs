namespace Client.Tracing.Tracing.Tracers.Sprint.UseCase;

public interface ICreateSprintTraceCollector
{
    Task NotificationReceived(Type originClassType, Guid traceId, object notification);
    Task AggregateAdded(Type originClassType, Guid traceId);
}