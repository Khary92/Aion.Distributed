namespace Core.Server.Tracing.Tracing.Tracers.TimerSettings.UseCase;

public interface IChangeSnapshotSaveIntervalTraceCollector
{
    Task CommandReceived(Type originClassType, Guid traceId, object protoCommand);
    Task EventPersisted(Type originClassType, Guid traceId, object @event);
    Task SendingNotification(Type originClassType, Guid traceId, object notification);
}