namespace Core.Server.Tracing.Tracing.Tracers.Ticket.UseCase;

public interface ICreateTicketTraceCollector
{
    Task CommandReceived(Type originClassType, Guid traceId, object @event);
    Task NotificationSent(Type originClassType, Guid traceId, object notification);
}