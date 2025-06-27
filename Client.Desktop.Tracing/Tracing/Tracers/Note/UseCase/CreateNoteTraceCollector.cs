namespace Client.Desktop.Tracing.Tracing.Tracers.Note.UseCase;

public class CreateNoteTraceCollector() : ICreateNoteTraceCollector
{
    public void StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes)
    {
        var log = $"Note creation requested {attributes}";
        //sink.AddTrace(DateTimeOffset.Now, LoggingMeta.ActionRequested, Guid.Empty, "toBeReplaced", originClassType,
        //    log);
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

    public void ExceptionOccured(Type originClassType, Guid traceId, Exception exception)
    {
        var log = $"Exception occured {exception}";
        //sink.AddTrace(DateTimeOffset.Now, LoggingMeta.ActionRequested, traceId, "toBeReplaced", originClassType, log);
    }
}