namespace Client.Desktop.Tracing.Tracing.Tracers.NoteType.UseCase;

public class ChangeNoteTypeNameTraceCollector() : IChangeNoteTypeNameTraceCollector
{
    public void StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes)
    {
        var log = $"Change Name requested for {attributes}";
        //sink.AddTrace(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId, "toBeReplaced", originClassType, log);
    }

    public void CommandSent(Type originClassType, Guid traceId, object command)
    {
        var log = ($"Sent {command}");
        //sink.AddTrace(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId, "toBeReplaced", originClassType, log);
    }

    public void NotificationReceived(Type originClassType, Guid traceId, object notification)
    {
        var log = ($"Received {notification}");
        //sink.AddTrace(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId, "toBeReplaced", originClassType, log);
    }

    public void NoAggregateFound(Type originClassType, Guid traceId)
    {
        var log = ($"Aggregate not found id:{traceId}");
        //sink.AddTrace(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId, "toBeReplaced", originClassType, log);
    }

    public void ChangesApplied(Type originClassType, Guid traceId)
    {
        var log = ($"Changed applied id:{traceId}");
        //sink.AddTrace(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId, "toBeReplaced", originClassType, log);
    }
}