using Client.Desktop.Communication.RequiresChange.Tracers.Ticket.UseCase;

namespace Client.Desktop.Communication.RequiresChange.Tracers.Ticket;

public class TicketUseCaseSelector(
    ICreateTicketTraceCollector createCollector,
    IUpdateTicketTraceCollector updateCollector,
    IAddTicketToCurrentSprintTraceCollector addTicketToSprintCollector) : ITicketUseCaseSelector
{
    public ICreateTicketTraceCollector Create => createCollector;
    public IUpdateTicketTraceCollector Update => updateCollector;
    public IAddTicketToCurrentSprintTraceCollector AddTicketToSprint => addTicketToSprintCollector;
}