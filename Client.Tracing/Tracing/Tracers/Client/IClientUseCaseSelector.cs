using Client.Tracing.Tracing.Tracers.Client.UseCase;

namespace Client.Tracing.Tracing.Tracers.Client;

public interface IClientUseCaseSelector
{
    ICreateTrackingControlCollector CreateTrackingControl { get; }
}