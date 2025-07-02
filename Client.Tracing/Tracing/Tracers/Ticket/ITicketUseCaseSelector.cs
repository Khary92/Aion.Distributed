using Client.Tracing.Tracing.Tracers.Ticket.UseCase;

namespace Client.Tracing.Tracing.Tracers.Ticket;

public interface ITicketUseCaseSelector
{
    ICreateTicketTraceCollector Create { get; }
    IUpdateTicketTraceCollector Update { get; }
    IAddTicketToCurrentSprintTraceCollector AddTicketToSprint { get; }
}