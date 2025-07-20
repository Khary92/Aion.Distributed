using Service.Admin.Tracing.Tracing.Sprint;
using Service.Admin.Tracing.Tracing.Tag;
using Service.Admin.Tracing.Tracing.Ticket;

namespace Service.Admin.Tracing;

public class TraceCollector(
    ITicketUseCaseSelector ticketUseCaseSelector, ISprintUseCaseSelector sprintUseCaseSelector, ITagUseCaseSelector tagUseCaseSelector) : ITraceCollector
{
    public ITicketUseCaseSelector Ticket => ticketUseCaseSelector;
    public ISprintUseCaseSelector Sprint => sprintUseCaseSelector;
    public ITagUseCaseSelector Tag => tagUseCaseSelector;
}