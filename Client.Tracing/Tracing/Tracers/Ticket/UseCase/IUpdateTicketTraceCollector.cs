namespace Client.Tracing.Tracing.Tracers.Ticket.UseCase;

public interface IUpdateTicketTraceCollector
{
    Task NotificationReceived(Type originClassType, Guid traceId, object command);
    Task NoAggregateFound(Type originClassType, Guid traceId);
    Task ChangesApplied(Type originClassType, Guid traceId);
}