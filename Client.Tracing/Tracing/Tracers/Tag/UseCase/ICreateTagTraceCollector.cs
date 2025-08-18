namespace Client.Tracing.Tracing.Tracers.Tag.UseCase;

public interface ICreateTagTraceCollector
{
    Task NotificationReceived(Type originClassType, Guid traceId, object notification);
    Task AggregateAdded(Type originClassType, Guid traceId);
}