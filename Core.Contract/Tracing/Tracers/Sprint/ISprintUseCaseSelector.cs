using Contract.Tracing.Tracers.Sprint.UseCase;

namespace Contract.Tracing.Tracers.Sprint;

public interface ISprintUseCaseSelector
{
    ICreateSprintTraceCollector Create { get; }
    IUpdateSprintCollector Update { get; }
    ISprintActiveStatusCollector ActiveStatus { get; }
    ITicketAddedToSprintCollector AddTicketToSprint { get; }
}