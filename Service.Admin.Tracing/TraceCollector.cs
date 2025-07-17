using Service.Admin.Tracing.Tracing.Ticket;

namespace Service.Admin.Tracing;

public class TraceCollector(
    ITicketUseCaseSelector ticketUseCaseSelector) : ITraceCollector
{
    public ITicketUseCaseSelector Ticket => ticketUseCaseSelector;
}