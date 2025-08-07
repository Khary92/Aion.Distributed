namespace Client.Tracing.Tracing.Tracers.Ticket.UseCase;

public interface IUpdateTicketDocuTraceCollector
{
    Task StartUseCase(Type originClassType, Guid traceId);
    Task CommandSent(Type originClassType, Guid traceId, object command);
    Task AggregateReceived(Type originClassType, Guid traceId, string attributes);
    Task AggregateAdded(Type originClassType, Guid traceId);
    Task NotificationReceived(Type originClassType, Guid traceId, object notification);
}