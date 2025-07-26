using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Service.Monitoring.Tracers.Sprint;

public class SprintTraceSink : ITraceSink
{
    public TraceSinkId TraceSinkId => TraceSinkId.Sprint;

    public void AddTrace(TraceData traceData)
    {
    }
}