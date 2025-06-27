namespace Client.Desktop.Tracing.Tracing.Tracers.Note.UseCase;

public interface ICreateNoteTraceCollector
{
    void StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes);
    void CommandSent(Type originClassType, Guid traceId, object command);
    void AggregateReceived(Type originClassType, Guid traceId, Dictionary<string, string> attributes);
    void AggregateAdded(Type originClassType, Guid traceId);
    void ExceptionOccured(Type originClassType, Guid traceId, Exception exception);
}