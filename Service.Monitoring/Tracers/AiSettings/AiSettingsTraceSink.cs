using Client.Desktop.Proto.Tracing.Enums;
using Proto.Command.TraceData;

namespace Service.Monitoring.Tracers.AiSettings;

public class AiSettingsTraceSink : ITraceSink
{
    public TraceSinkId TraceSinkId => TraceSinkId.AiSettings;

    public void AddTrace(TraceDataCommandProto traceDataCommandProto)
    {
    }
}