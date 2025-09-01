namespace Core.Server.Tracing.Tracing.Tracers.WorkDay.UseCase;

public interface ICreateWorkDayTraceCollector
{
    Task CommandReceived(Type originClassType, Guid traceId, object protoCommand);
    Task EventPersisted(Type originClassType, Guid traceId, object @event);
    Task SendingNotification(Type originClassType, Guid traceId, object notification);
    Task ActionAborted(Type originClassType, Guid traceId);
}