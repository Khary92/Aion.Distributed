namespace Core.Server.Tracing.Tracing.Tracers.Ticket.UseCase;

public interface IUpdateTicketTraceCollector
{
    Task CommandReceived(Type getType, Guid parse, object command);
    Task EventPersisted(Type originClassType, Guid traceId, object @event);
    Task NotificationSent(Type originClassType, Guid traceId, object notification);
}