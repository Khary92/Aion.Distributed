namespace Client.Desktop.Tracing.Tracing.Tracers.TimerSettings.UseCase;

public interface ICreateTimerSettingsTraceCollector
{
    Task StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes);
    Task CommandSent(Type originClassType, Guid traceId, object command);
    Task AggregateReceived(Type originClassType, Guid traceId, Dictionary<string, string> attributes);
    Task AggregateAdded(Type originClassType, Guid traceId);
}