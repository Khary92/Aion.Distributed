using Client.Tracing.Tracing.Tracers.Ticket.UseCase;

namespace Client.Tracing.Tracing.Tracers.Ticket;

public interface ITicketUseCaseSelector
{
    IUpdateTicketDocuTraceCollector Documentation { get; }
    ICreateTicketUseCaseCollector Create { get; }
    IUpdateTicketTraceCollector Update { get; }
}