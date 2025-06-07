namespace Contract.Tracing.Tracers.TimerSettings.UseCase;

public interface ICreateTimerSettingsTraceCollector
{
    void StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes);
    void CommandSent(Type originClassType, Guid traceId, object command);
    void AggregateReceived(Type originClassType, Guid traceId, Dictionary<string, string> attributes);
    void AggregateAdded(Type originClassType, Guid traceId);
}