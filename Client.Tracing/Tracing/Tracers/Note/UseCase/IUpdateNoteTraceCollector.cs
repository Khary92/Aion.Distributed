namespace Client.Tracing.Tracing.Tracers.Note.UseCase;

public interface IUpdateNoteTraceCollector
{
    Task NotificationReceived(Type originClassType, Guid traceId, object notification);
    Task NoAggregateFound(Type originClassType, Guid traceId);
    Task ChangesApplied(Type originClassType, Guid traceId);
    Task ExceptionOccured(Type originClassType, Guid traceId, Exception exception);
}