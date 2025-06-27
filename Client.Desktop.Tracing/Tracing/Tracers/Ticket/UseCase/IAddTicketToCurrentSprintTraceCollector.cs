namespace Client.Desktop.Tracing.Tracing.Tracers.Ticket.UseCase;

public interface IAddTicketToCurrentSprintTraceCollector
{
    void StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes);
    void CommandSent(Type originClassType, Guid traceId, object command);
}