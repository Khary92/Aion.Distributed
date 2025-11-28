using Service.Monitoring.Shared;

namespace Client.Tracing;

public interface ITracingDataSender
{
    Task<bool> Send(ServiceTraceDataCommand command);
}