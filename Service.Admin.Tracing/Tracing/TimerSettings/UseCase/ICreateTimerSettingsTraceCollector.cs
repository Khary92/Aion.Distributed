namespace Service.Admin.Tracing.Tracing.TimerSettings.UseCase;

public interface ICreateTimerSettingsTraceCollector
{
    Task StartUseCase(Type originClassType, Guid traceId, string attributes);
    Task CommandSent(Type originClassType, Guid traceId, object command);
    Task AggregateReceived(Type originClassType, Guid traceId, string attributes);
    Task AggregateAdded(Type originClassType, Guid traceId);
}