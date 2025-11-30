using Service.Monitoring.Shared;

namespace Core.Server.Communication.Tracing;

public interface ITracingDataSender
{
    Task<bool> Send(ServiceTraceDataCommand command);
}