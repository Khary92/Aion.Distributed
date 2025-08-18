namespace Client.Tracing.Tracing.Tracers.Ticket.UseCase;

public interface IUpdateTicketDocuTraceCollector
{
    Task StartUseCase(Type originClassType, Guid traceId);
    Task SendingCommand(Type originClassType, Guid traceId, object command);
    Task AggregateAdded(Type originClassType, Guid traceId);
    Task NotificationReceived(Type originClassType, Guid traceId, object notification);

}