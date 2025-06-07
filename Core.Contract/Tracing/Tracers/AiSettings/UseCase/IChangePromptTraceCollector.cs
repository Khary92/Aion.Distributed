namespace Contract.Tracing.Tracers.AiSettings.UseCase;

public interface IChangePromptTraceCollector
{
    void StartUseCase(Type originClassType, Guid traceId, (string, string) property);
    void CommandSent(Type originClassType, Guid traceId, object command);
    void PropertyNotChanged(Type originClassType, Guid traceId, (string, string) property);
}