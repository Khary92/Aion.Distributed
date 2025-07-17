using Service.Admin.Tracing.Tracing.Ticket;

namespace Service.Admin.Tracing;

public interface ITraceCollector
{
    ITicketUseCaseSelector Ticket { get; }
}