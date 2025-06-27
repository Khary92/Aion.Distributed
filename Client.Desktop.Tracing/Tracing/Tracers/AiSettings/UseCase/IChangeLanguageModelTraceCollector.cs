namespace Client.Desktop.Tracing.Tracing.Tracers.AiSettings.UseCase;

public interface IChangeLanguageModelTraceCollector
{
    Task StartUseCase(Type originClassType, Guid traceId, (string, string) attribute);
    Task CommandSent(Type originClassType, Guid traceId, object command);
    Task PropertyNotChanged(Type originClassType, Guid traceId, (string, string) attribute);
}