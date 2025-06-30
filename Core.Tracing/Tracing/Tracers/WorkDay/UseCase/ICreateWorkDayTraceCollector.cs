namespace Core.Server.Tracing.Tracing.Tracers.WorkDay.UseCase;

public interface ICreateWorkDayTraceCollector
{
    Task StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes);
    Task CommandSent(Type originClassType, Guid traceId, object command);
    Task AggregateReceived(Type originClassType, Guid traceId, Dictionary<string, string> attributes);
    Task AggregateAdded(Type originClassType, Guid traceId);
}