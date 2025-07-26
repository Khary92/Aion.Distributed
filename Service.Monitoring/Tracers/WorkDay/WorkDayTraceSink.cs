using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Service.Monitoring.Tracers.WorkDay;

public class WorkDayTraceSink : ITraceSink
{
    public TraceSinkId TraceSinkId => TraceSinkId.WorkDay;

    public void AddTrace(TraceData traceData)
    {
    }
}