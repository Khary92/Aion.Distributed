namespace Core.Server.Tracing.Tracing.Tracers.Note.UseCase;

public interface IUpdateNoteTraceCollector
{
    Task StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes);
    Task CommandSent(Type originClassType, Guid traceId, object command);
    Task NotificationReceived(Type originClassType, Guid traceId, object notification);
    Task NoAggregateFound(Type originClassType, Guid traceId);
    Task ChangesApplied(Type originClassType, Guid traceId);
    Task ExceptionOccured(Type originClassType, Guid traceId, Exception exception);
}