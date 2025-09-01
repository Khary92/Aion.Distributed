namespace Client.Tracing.Tracing.Tracers.Statistics.UseCase;

public interface IChangeTagSelectionTraceCollector
{
    Task StartUseCase(Type originClassType, Guid traceId);
    Task SendingCommand(Type originClassType, Guid traceId, object command);
    Task NotificationReceived(Type originClassType, Guid traceId, object notification);
    Task NoAggregateFound(Type originClassType, Guid traceId);
    Task ChangesApplied(Type originClassType, Guid traceId);
    Task WrongModel(Type originClassType, Guid traceId);
}