using Core.Server.Tracing.Tracing.Tracers.Ticket.UseCase;

namespace Core.Server.Tracing.Tracing.Tracers.Ticket;

public interface ITicketUseCaseSelector
{
    ICreateTicketTraceCollector Create { get; }
    IUpdateTicketTraceCollector Update { get; }
    IChangeDocumentationTraceCollector ChangeDocumentation { get; }
}