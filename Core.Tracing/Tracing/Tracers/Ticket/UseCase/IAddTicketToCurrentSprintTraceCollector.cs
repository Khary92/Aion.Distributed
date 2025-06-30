namespace Core.Server.Tracing.Tracing.Tracers.Ticket.UseCase;

public interface IAddTicketToCurrentSprintTraceCollector
{
    Task StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes);
    Task CommandSent(Type originClassType, Guid traceId, object command);
}