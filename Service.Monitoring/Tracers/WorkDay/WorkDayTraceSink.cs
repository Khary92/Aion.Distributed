using Client.Desktop.Proto.Tracing.Enums;
using Proto.Command.TraceData;

namespace Service.Monitoring.Tracers.WorkDay;

public class WorkDayTraceSink : ITraceSink
{
    public TraceSinkId TraceSinkId => TraceSinkId.WorkDay;

    public void AddTrace(TraceDataCommandProto traceDataCommandProto)
    {
    }
}