using Client.Desktop.Tracing.Tracing.Tracers.Sprint.UseCase;

namespace Client.Desktop.Tracing.Tracing.Tracers.Sprint;

public class SprintUseCaseSelector(
    ICreateSprintTraceCollector createSprintTraceCollector,
    IUpdateSprintCollector  updateSprintCollector,
    ISprintActiveStatusCollector sprintActiveStatusCollector,
    ITicketAddedToSprintCollector  ticketAddedToSprintCollector
) : ISprintUseCaseSelector
{
    public ICreateSprintTraceCollector Create => createSprintTraceCollector;
    public IUpdateSprintCollector Update => updateSprintCollector;
    public ISprintActiveStatusCollector ActiveStatus => sprintActiveStatusCollector;
    public ITicketAddedToSprintCollector AddTicketToSprint => ticketAddedToSprintCollector;
}