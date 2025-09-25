using Core.Server.Tracing.Tracing.Tracers.Ticket.UseCase;

namespace Core.Server.Tracing.Tracing.Tracers.Ticket;

public class TicketUseCaseSelector(
    ICreateTicketTraceCollector createCollector,
    IUpdateTicketTraceCollector updateCollector,
    IChangeDocumentationTraceCollector documentationTraceCollector) : ITicketUseCaseSelector
{
    public ICreateTicketTraceCollector Create => createCollector;
    public IUpdateTicketTraceCollector Update => updateCollector;
    public IChangeDocumentationTraceCollector ChangeDocumentation => documentationTraceCollector;
}