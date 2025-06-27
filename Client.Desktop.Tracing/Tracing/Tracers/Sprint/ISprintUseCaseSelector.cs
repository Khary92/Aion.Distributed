using Client.Desktop.Tracing.Tracing.Tracers.Sprint.UseCase;

namespace Client.Desktop.Tracing.Tracing.Tracers.Sprint;

public interface ISprintUseCaseSelector
{
    ICreateSprintTraceCollector Create { get; }
    IUpdateSprintCollector Update { get; }
    ISprintActiveStatusCollector ActiveStatus { get; }
    ITicketAddedToSprintCollector AddTicketToSprint { get; }
}