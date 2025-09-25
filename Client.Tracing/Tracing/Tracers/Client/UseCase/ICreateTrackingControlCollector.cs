namespace Client.Tracing.Tracing.Tracers.Client.UseCase;

public interface ICreateTrackingControlCollector
{
    Task StartUseCase(Type originClassType, Guid traceId);
    Task SendingCommand(Type originClassType, Guid traceId, object command);
    Task NotificationReceived(Type originClassType, Guid traceId, object notification);
    Task AggregateAdded(Type originClassType, Guid traceId);
    Task ExceptionOccured(Type originClassType, Guid traceId, Exception exception);
}