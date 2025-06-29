using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Service.Monitoring.Tracers.AiSettings;

public class AiSettingsTraceSink : ITraceSink
{
    public TraceSinkId TraceSinkId => TraceSinkId.AiSettings;

    public void AddTrace(TraceData traceData)
    {
    }
}