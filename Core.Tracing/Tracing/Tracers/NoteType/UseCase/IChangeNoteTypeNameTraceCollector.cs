namespace Core.Server.Tracing.Tracing.Tracers.NoteType.UseCase;

public interface IChangeNoteTypeNameTraceCollector
{
    Task CommandReceived(Type originClassType, Guid traceId, object protoCommand);
    Task EventPersisted(Type originClassType, Guid traceId, object @event);
    Task SendingNotification(Type originClassType, Guid traceId, object notification);
}