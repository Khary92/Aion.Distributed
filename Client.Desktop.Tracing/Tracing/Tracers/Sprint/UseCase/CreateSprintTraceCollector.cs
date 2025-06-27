namespace Client.Desktop.Tracing.Tracing.Tracers.Sprint.UseCase;

public class CreateSprintTraceCollector() : ICreateSprintTraceCollector
{
    public void StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes)
    {
        var log = $"Create Sprint requested for {attributes}";
        //sink.AddTrace(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId, "toBeReplaced", originClassType, log);
    }

    public void CommandSent(Type originClassType, Guid traceId, object command)
    {
        var log = ($"Sent {command}");
        //sink.AddTrace(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId, "toBeReplaced", originClassType, log);
    }

    public void AggregateReceived(Type originClassType, Guid traceId, Dictionary<string, string> attributes)
    {
        var log = ($"Received aggregate {attributes}");
        //sink.AddTrace(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId, "toBeReplaced", originClassType, log);
    }

    public void AggregateAdded(Type originClassType, Guid traceId)
    {
        var log = ($"Added aggregate with id:{traceId}");
        //sink.AddTrace(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId, "toBeReplaced", originClassType, log);
    }
}