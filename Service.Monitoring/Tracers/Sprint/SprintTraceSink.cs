using Client.Desktop.Proto.Tracing.Enums;
using Proto.Command.TraceData;

namespace Service.Monitoring.Tracers.Sprint;

public class SprintTraceSink : ITraceSink
{
    public TraceSinkId TraceSinkId => TraceSinkId.Sprint;

    public void AddTrace(TraceDataCommandProto traceDataCommandProto)
    {
    }
}