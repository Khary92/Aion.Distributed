using Service.Admin.Tracing.Tracing.Ticket.UseCase;

namespace Service.Admin.Tracing.Tracing.Ticket;

public interface ITicketUseCaseSelector
{
    ICreateTicketTraceCollector Create { get; }
    IUpdateTicketTraceCollector Update { get; }
    IChangeDocumentationTraceCollector ChangeDocumentation { get; }
}