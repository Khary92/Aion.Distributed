namespace Service.Monitoring.Shared.Tracing;

public interface ITracingDataSender
{
    Task<bool> Send(ServiceTraceDataCommand command);
}