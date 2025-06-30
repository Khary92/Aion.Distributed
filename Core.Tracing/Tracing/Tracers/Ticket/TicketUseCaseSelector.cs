using Core.Server.Tracing.Tracing.Tracers.Ticket.UseCase;

namespace Core.Server.Tracing.Tracing.Tracers.Ticket;

public class TicketUseCaseSelector(
    ICreateTicketTraceCollector createCollector,
    IUpdateTicketTraceCollector updateCollector,
    IAddTicketToCurrentSprintTraceCollector addTicketToSprintCollector) : ITicketUseCaseSelector
{
    public ICreateTicketTraceCollector Create => createCollector;
    public IUpdateTicketTraceCollector Update => updateCollector;
    public IAddTicketToCurrentSprintTraceCollector AddTicketToSprint => addTicketToSprintCollector;
}