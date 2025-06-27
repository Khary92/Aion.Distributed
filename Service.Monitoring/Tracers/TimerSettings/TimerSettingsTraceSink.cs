using Client.Desktop.Proto.Tracing.Enums;
using Proto.Command.TraceData;

namespace Service.Monitoring.Tracers.TimerSettings;

public class TimerSettingsTraceSink : ITraceSink
{
    public TraceSinkId TraceSinkId => TraceSinkId.TimerSettings;

    public void AddTrace(TraceDataCommandProto traceDataCommandProto)
    {
    }
}