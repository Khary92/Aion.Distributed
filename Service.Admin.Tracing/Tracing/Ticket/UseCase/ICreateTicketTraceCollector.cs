namespace Service.Admin.Tracing.Tracing.Ticket.UseCase;

public interface ICreateTicketTraceCollector
{
    Task StartUseCase(Type originClassType, Guid traceId, object command);
    Task SendingCommand(Type originClassType, Guid traceId, object command);
    Task AggregateReceived(Type originClassType, Guid traceId, object command);
    Task AggregateAdded(Type originClassType, Guid traceId);
    Task NotificationReceived(Type originClassType, Guid traceId, object notification);
}