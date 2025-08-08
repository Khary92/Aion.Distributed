namespace Client.Tracing.Tracing.Tracers.TimeSlot.UseCase;

public interface ISetStartTimeTraceCollector
{
    Task StartUseCase(Type originClassType, Guid traceId);
    Task SendingCommand(Type originClassType, Guid traceId, object command);
    Task CacheIsEmpty(Type originClassType, Guid traceId);
}