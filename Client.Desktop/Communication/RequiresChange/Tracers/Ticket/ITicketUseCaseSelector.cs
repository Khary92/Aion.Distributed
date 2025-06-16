using Client.Desktop.Communication.RequiresChange.Tracers.Ticket.UseCase;

namespace Client.Desktop.Communication.RequiresChange.Tracers.Ticket;

public interface ITicketUseCaseSelector
{
    ICreateTicketTraceCollector Create { get; }
    IUpdateTicketTraceCollector Update { get; }
    IAddTicketToCurrentSprintTraceCollector AddTicketToSprint { get; }
}