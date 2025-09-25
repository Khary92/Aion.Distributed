using Core.Server.Tracing.Tracing.Tracers.Client.UseCase;

namespace Core.Server.Tracing.Tracing.Tracers.Client;

public class ClientUseCaseSelector(ICreateTrackingControlCollector createTrackingControlCollector)
    : IClientUseCaseSelector
{
    public ICreateTrackingControlCollector CreateTrackingControl => createTrackingControlCollector;
}