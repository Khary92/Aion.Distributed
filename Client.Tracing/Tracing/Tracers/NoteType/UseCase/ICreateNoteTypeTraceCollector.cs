namespace Client.Tracing.Tracing.Tracers.NoteType.UseCase;

public interface ICreateNoteTypeTraceCollector
{
    Task NotificationReceived(Type originClassType, Guid traceId, object notification);
    Task AggregateAdded(Type originClassType, Guid traceId);
}