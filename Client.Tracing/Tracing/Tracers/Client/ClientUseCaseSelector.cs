using Client.Tracing.Tracing.Tracers.Client.UseCase;

namespace Client.Tracing.Tracing.Tracers.Client;

public class ClientUseCaseSelector(
    ICreateTrackingControlCollector createNoteTraceCollector) : IClientUseCaseSelector
{
    public ICreateTrackingControlCollector CreateTrackingControl => createNoteTraceCollector;
}