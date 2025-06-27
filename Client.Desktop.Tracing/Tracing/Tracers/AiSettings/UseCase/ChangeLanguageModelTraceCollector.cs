namespace Client.Desktop.Tracing.Tracing.Tracers.AiSettings.UseCase;

public class ChangeLanguageModelTraceCollector()
    : IChangeLanguageModelTraceCollector
{
    public void StartUseCase(Type originClassType, Guid traceId, (string, string) attribute)
    {
        var log = ($"Change LanguageModel requested for {attribute.Item1}:{attribute.Item2}");

        //sink.AddTrace(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId, "toBeReplaced", originClassType, log);
    }

    public void CommandSent(Type originClassType, Guid traceId, object command)
    {
        var log = ($"Sent {command}");

        //sink.AddTrace(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId, "toBeReplaced", originClassType, log);
    }

    public void PropertyNotChanged(Type originClassType, Guid traceId, (string, string) attribute)
    {
        var log = ($"Request aborted {attribute.Item1}:{attribute.Item2}");

        //sink.AddTrace(DateTimeOffset.Now, LoggingMeta.PropertyNotChanged, traceId, "toBeReplaced", originClassType,
        //    log);
    }
}