namespace Client.Tracing.Tracing.Tracers.Ticket.UseCase;

public interface IAddTicketToCurrentSprintTraceCollector
{
    Task StartUseCase(Type originClassType, Guid traceId, string attributes);
    Task CommandSent(Type originClassType, Guid traceId, object command);
}