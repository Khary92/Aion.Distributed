namespace Service.Admin.Tracing.Tracing.Tag.UseCase;

public interface ICreateTagTraceCollector
{
    Task StartUseCase(Type originClassType, Guid traceId);
    Task SendingCommand(Type originClassType, Guid traceId, object command);
    Task AggregateReceived(Type originClassType, Guid traceId, string attributes);
    Task AggregateAdded(Type originClassType, Guid traceId);
}