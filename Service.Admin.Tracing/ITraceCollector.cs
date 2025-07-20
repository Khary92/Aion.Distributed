using Service.Admin.Tracing.Tracing.Sprint;
using Service.Admin.Tracing.Tracing.Tag;
using Service.Admin.Tracing.Tracing.Ticket;

namespace Service.Admin.Tracing;

public interface ITraceCollector
{
    ITicketUseCaseSelector Ticket { get; }
    ISprintUseCaseSelector Sprint { get; }
    ITagUseCaseSelector Tag { get; }
}