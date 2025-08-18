namespace Client.Tracing.Tracing.Tracers.Note.UseCase;

public interface ICreateNoteTraceCollector
{
    Task NotificationReceived(Type originClassType, Guid traceId, object notification);
    Task AggregateAdded(Type originClassType, Guid traceId);
    Task ExceptionOccured(Type originClassType, Guid traceId, Exception exception);
}