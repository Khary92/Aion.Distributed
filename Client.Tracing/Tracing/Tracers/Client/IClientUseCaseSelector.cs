using Client.Tracing.Tracing.Tracers.Client.UseCase;
using Client.Tracing.Tracing.Tracers.Note.UseCase;

namespace Client.Tracing.Tracing.Tracers.Client;

public interface IClientUseCaseSelector
{
    ICreateTrackingControlCollector CreateTrackingControl { get; }
}