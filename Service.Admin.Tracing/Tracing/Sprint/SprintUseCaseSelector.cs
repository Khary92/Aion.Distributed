using Service.Admin.Tracing.Tracing.Sprint.UseCase;

namespace Service.Admin.Tracing.Tracing.Sprint;

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