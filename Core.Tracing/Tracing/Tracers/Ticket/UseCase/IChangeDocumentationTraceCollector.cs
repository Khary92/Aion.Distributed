namespace Core.Server.Tracing.Tracing.Tracers.Ticket.UseCase;

public interface IChangeDocumentationTraceCollector
{
    Task CommandReceived(Type getType, Guid parse, object command);
    Task EventPersisted(Type originClassType, Guid traceId, object @event);
    Task SendingNotification(Type originClassType, Guid traceId, object notification);
}