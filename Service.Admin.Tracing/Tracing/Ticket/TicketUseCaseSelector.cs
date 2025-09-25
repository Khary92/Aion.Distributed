using Service.Admin.Tracing.Tracing.Ticket.UseCase;

namespace Service.Admin.Tracing.Tracing.Ticket;

public class TicketUseCaseSelector(
    ICreateTicketTraceCollector createCollector,
    IUpdateTicketTraceCollector updateCollector,
    IChangeDocumentationTraceCollector documentationCollector) : ITicketUseCaseSelector
{
    public ICreateTicketTraceCollector Create => createCollector;
    public IUpdateTicketTraceCollector Update => updateCollector;
    public IChangeDocumentationTraceCollector ChangeDocumentation => documentationCollector;
}