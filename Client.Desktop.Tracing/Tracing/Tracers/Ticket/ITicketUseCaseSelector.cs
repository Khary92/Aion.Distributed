using Client.Desktop.Tracing.Tracing.Tracers.Ticket.UseCase;

namespace Client.Desktop.Tracing.Tracing.Tracers.Ticket;

public interface ITicketUseCaseSelector
{
    ICreateTicketTraceCollector Create { get; }
    IUpdateTicketTraceCollector Update { get; }
    IAddTicketToCurrentSprintTraceCollector AddTicketToSprint { get; }
}