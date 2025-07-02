using Client.Tracing.Tracing.Tracers.Sprint.UseCase;

namespace Client.Tracing.Tracing.Tracers.Sprint;

public interface ISprintUseCaseSelector
{
    ICreateSprintTraceCollector Create { get; }
    IUpdateSprintCollector Update { get; }
    ISprintActiveStatusCollector ActiveStatus { get; }
    ITicketAddedToSprintCollector AddTicketToSprint { get; }
}