namespace Service.Admin.Tracing.Tracing.Ticket.UseCase;

public interface IChangeDocumentationTraceCollector
{
    Task NotificationReceived(Type originClassType, Guid traceId, object notification);
    Task NoAggregateFound(Type originClassType, Guid traceId);
    Task ChangesApplied(Type originClassType, Guid traceId);
}