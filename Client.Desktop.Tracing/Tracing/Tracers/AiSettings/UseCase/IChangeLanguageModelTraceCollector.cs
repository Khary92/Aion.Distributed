namespace Client.Desktop.Tracing.Tracing.Tracers.AiSettings.UseCase;

public interface IChangeLanguageModelTraceCollector
{
    void StartUseCase(Type originClassType, Guid traceId, (string, string) attribute);
    void CommandSent(Type originClassType, Guid traceId, object command);
    void PropertyNotChanged(Type originClassType, Guid traceId, (string, string) attribute);
}