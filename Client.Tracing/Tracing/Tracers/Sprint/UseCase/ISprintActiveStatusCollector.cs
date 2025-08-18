namespace Client.Tracing.Tracing.Tracers.Sprint.UseCase;

public interface ISprintActiveStatusCollector
{
    Task NotificationReceived(Type originClassType, Guid traceId, object notification);
    Task NoAggregateFound(Type originClassType, Guid traceId);
    Task ChangesApplied(Type originClassType, Guid traceId);
}