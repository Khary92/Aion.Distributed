using Client.Desktop.Tracing.Tracing;

namespace Client.Desktop.Tracing.Communication.Tracing;

public interface ITracingDataCommandSender
{
    Task<bool> Send(TraceDataCommand command);
}