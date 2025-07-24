namespace Service.Admin.Tracing.Tracing.Ticket.UseCase;

public interface IAddTicketToCurrentSprintTraceCollector
{
    Task StartUseCase(Type originClassType, Guid traceId, string attributes);
    Task CommandSent(Type originClassType, Guid traceId, object command);
}