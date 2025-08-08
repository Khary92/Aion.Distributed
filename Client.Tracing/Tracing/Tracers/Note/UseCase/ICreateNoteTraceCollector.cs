namespace Client.Tracing.Tracing.Tracers.Note.UseCase;

public interface ICreateNoteTraceCollector
{
    Task StartUseCase(Type originClassType, Guid traceId);
    Task SendingCommand(Type originClassType, Guid traceId, object command);
    Task AggregateReceived(Type originClassType, Guid traceId, string attributes);
    Task AggregateAdded(Type originClassType, Guid traceId);
    Task ExceptionOccured(Type originClassType, Guid traceId, Exception exception);
}