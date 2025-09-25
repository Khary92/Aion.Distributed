using Client.Tracing.Tracing.Tracers.Ticket.UseCase;

namespace Client.Tracing.Tracing.Tracers.Ticket;

public class TicketUseCaseSelector(
    IUpdateTicketDocuTraceCollector updateCollector,
    ICreateTicketUseCaseCollector createTicketUseCaseCollector,
    IUpdateTicketTraceCollector updateTicketTraceCollector) : ITicketUseCaseSelector
{
    public IUpdateTicketDocuTraceCollector ChangeDocumentation => updateCollector;
    public ICreateTicketUseCaseCollector Create => createTicketUseCaseCollector;
    public IUpdateTicketTraceCollector Update => updateTicketTraceCollector;
}