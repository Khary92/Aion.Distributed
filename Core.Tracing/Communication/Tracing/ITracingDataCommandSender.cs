using Service.Monitoring.Shared;

namespace Core.Server.Tracing.Communication.Tracing;

public interface ITracingDataCommandSender
{
    Task<bool> Send(ServiceTraceDataCommand command);
}