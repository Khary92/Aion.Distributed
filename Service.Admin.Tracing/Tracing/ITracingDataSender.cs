using Service.Monitoring.Shared;

namespace Service.Admin.Tracing.Tracing;

public interface ITracingDataSender
{
    Task<bool> Send(ServiceTraceDataCommand command);
}