using Proto.Command.TraceData;
using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Service.Monitoring.Tracers.Tag;

public class TagTraceSink : ITraceSink
{
    public TraceSinkId TraceSinkId => TraceSinkId.Tag;

    public void AddTrace(TraceData traceData)
    {
    }
}