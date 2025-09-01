namespace Service.Admin.Tracing.Tracing.NoteType.UseCase;

public interface ICreateNoteTypeTraceCollector
{
    Task StartUseCase(Type originClassType, Guid traceId, string attributes);
    Task SendingCommand(Type originClassType, Guid traceId, object command);
    Task NotificationReceived(Type originClassType, Guid traceId, object notification);
    Task AggregateAdded(Type originClassType, Guid traceId);
}