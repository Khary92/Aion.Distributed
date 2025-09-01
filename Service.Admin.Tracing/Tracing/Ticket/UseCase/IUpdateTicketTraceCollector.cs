namespace Service.Admin.Tracing.Tracing.Ticket.UseCase;

public interface IUpdateTicketTraceCollector
{
    Task StartUseCase(Type originClassType, Guid traceId);
    Task NoEntitySelected(Type originClassType, Guid traceId);
    Task SendingCommand(Type originClassType, Guid traceId, object command);
    Task NotificationReceived(Type originClassType, Guid traceId, object notification);
    Task NoAggregateFound(Type originClassType, Guid traceId);
    Task ChangesApplied(Type originClassType, Guid traceId);
}