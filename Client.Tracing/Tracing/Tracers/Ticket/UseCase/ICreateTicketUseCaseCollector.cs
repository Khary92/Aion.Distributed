namespace Client.Tracing.Tracing.Tracers.Ticket.UseCase;

public interface ICreateTicketUseCaseCollector
{
    Task AggregateAdded(Type originClassType, Guid traceId);
    Task NotificationReceived(Type originClassType, Guid traceId, object notification);
}