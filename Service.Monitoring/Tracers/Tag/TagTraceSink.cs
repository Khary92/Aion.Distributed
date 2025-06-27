using Client.Desktop.Proto.Tracing.Enums;
using Proto.Command.TraceData;

namespace Service.Monitoring.Tracers.Tag;

public class TagTraceSink : ITraceSink
{
    public TraceSinkId TraceSinkId => TraceSinkId.Tag;

    public void AddTrace(TraceDataCommandProto traceDataCommandProto)
    {
    }
}