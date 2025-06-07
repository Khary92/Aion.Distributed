using Contract.Tracing.Tracers.Ticket.UseCase;

namespace Contract.Tracing.Tracers.Ticket;

public interface ITicketUseCaseSelector
{
    ICreateTicketTraceCollector Create { get; }
    IUpdateTicketTraceCollector Update { get; }
    IAddTicketToCurrentSprintTraceCollector AddTicketToSprint { get; }
}