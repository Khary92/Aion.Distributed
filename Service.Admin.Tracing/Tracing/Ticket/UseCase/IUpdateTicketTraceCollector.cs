namespace Service.Admin.Tracing.Tracing.Ticket.UseCase;

public interface IUpdateTicketTraceCollector
{
    Task StartUseCase(Type originClassType, Guid traceId, object command);
    Task CommandSent(Type originClassType, Guid traceId, object command);
    Task NotificationReceived(Type originClassType, Guid traceId, object command);
    Task NoAggregateFound(Type originClassType, Guid traceId);
    Task ChangesApplied(Type originClassType, Guid traceId);
}