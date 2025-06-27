namespace Client.Desktop.Tracing.Tracing.Tracers.Sprint.UseCase;

public interface ITicketAddedToSprintCollector
{
    void StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes);
    void CommandSent(Type originClassType, Guid traceId, object command);
    void NotificationReceived(Type originClassType, Guid traceId, object notification);
    void NoAggregateFound(Type originClassType, Guid traceId);
    void ChangesApplied(Type originClassType, Guid traceId);
}