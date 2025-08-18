namespace Client.Tracing.Tracing.Tracers.NoteType.UseCase;

public interface IChangeNoteTypeNameTraceCollector
{
    Task NotificationReceived(Type originClassType, Guid traceId, object notification);
    Task NoAggregateFound(Type originClassType, Guid traceId);
    Task ChangesApplied(Type originClassType, Guid traceId);
}