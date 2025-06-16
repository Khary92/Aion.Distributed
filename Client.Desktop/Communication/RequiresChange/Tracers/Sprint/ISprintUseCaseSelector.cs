using Client.Desktop.Communication.RequiresChange.Tracers.Sprint.UseCase;

namespace Client.Desktop.Communication.RequiresChange.Tracers.Sprint;

public interface ISprintUseCaseSelector
{
    ICreateSprintTraceCollector Create { get; }
    IUpdateSprintCollector Update { get; }
    ISprintActiveStatusCollector ActiveStatus { get; }
    ITicketAddedToSprintCollector AddTicketToSprint { get; }
}