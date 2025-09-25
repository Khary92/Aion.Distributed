using Core.Server.Tracing.Tracing.Tracers.Client.UseCase;

namespace Core.Server.Tracing.Tracing.Tracers.Client;

public interface IClientUseCaseSelector
{
    ICreateTrackingControlCollector CreateTrackingControl { get; }
}