namespace Client.Desktop.Tracing.Tracing.Tracers.AiSettings.UseCase;

public class ChangePromptTraceCollector() : IChangePromptTraceCollector
{
    public void StartUseCase(Type originClassType, Guid traceId, (string, string) attributes)
    {
        var log = ($"Change Prompt requested for {attributes.Item1}:{attributes.Item2}");
        //sink.AddTrace(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId, "toBeReplaced", originClassType, log);
    }

    public void CommandSent(Type originClassType, Guid traceId, object command)
    {
        var log = ($"Sent {command}");
        //sink.AddTrace(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId, "toBeReplaced", originClassType, log);
    }

    public void PropertyNotChanged(Type originClassType, Guid traceId, (string, string) property)
    {
        var log = ($"Request aborted {property.Item1}:{property.Item2}");
        //sink.AddTrace(DateTimeOffset.Now, LoggingMeta.PropertyNotChanged, traceId, "toBeReplaced", originClassType, log);
    }
}