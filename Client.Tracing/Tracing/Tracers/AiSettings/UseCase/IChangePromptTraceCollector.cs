namespace Client.Tracing.Tracing.Tracers.AiSettings.UseCase;

public interface IChangePromptTraceCollector
{
    Task StartUseCase(Type originClassType, Guid traceId, (string, string) property);
    Task CommandSent(Type originClassType, Guid traceId, object command);
    Task PropertyNotChanged(Type originClassType, Guid traceId, (string, string) property);
}