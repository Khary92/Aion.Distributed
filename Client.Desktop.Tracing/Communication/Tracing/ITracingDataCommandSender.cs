using Client.Desktop.Tracing.Tracing;
using Service.Monitoring.Shared;

namespace Client.Desktop.Tracing.Communication.Tracing;

public interface ITracingDataCommandSender
{
    Task<bool> Send(ServiceTraceDataCommand command);
}