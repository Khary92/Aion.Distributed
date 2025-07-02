namespace Service.Monitoring.Shared.Tracing;

public interface ITracingDataCommandSender
{
    Task<bool> Send(ServiceTraceDataCommand command);
}