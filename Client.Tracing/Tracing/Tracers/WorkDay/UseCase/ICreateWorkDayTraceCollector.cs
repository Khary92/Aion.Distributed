namespace Client.Tracing.Tracing.Tracers.WorkDay.UseCase;

public interface ICreateWorkDayTraceCollector
{
    Task StartUseCase(Type originClassType, Guid traceId);
    Task SendingCommand(Type originClassType, Guid traceId, object command);
    Task AggregateAdded(Type originClassType, Guid traceId);
    Task ActionAborted(Type originClassType, Guid traceId);
    Task NotificationReceived(Type originClassType, Guid traceId, object notification);
}