using Proto.Command.TraceData;
using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Service.Monitoring.Tracers.TimerSettings;

public class TimerSettingsTraceSink : ITraceSink
{
    public TraceSinkId TraceSinkId => TraceSinkId.TimerSettings;

    public void AddTrace(TraceData traceData)
    {
    }
}