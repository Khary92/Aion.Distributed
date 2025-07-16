namespace Client.Tracing.Tracing.Tracers.Sprint.UseCase;

public interface ISprintActiveStatusCollector
{
    Task StartUseCase(Type originClassType, Guid traceId, string attributes);
    Task CommandSent(Type originClassType, Guid traceId, object command);
    Task NotificationReceived(Type originClassType, Guid traceId, object notification);
    Task NoAggregateFound(Type originClassType, Guid traceId);
    Task ChangesApplied(Type originClassType, Guid traceId);
}