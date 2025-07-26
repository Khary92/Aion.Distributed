using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Service.Monitoring.Tracers.NoteType;

public class NoteTypeTraceSink : ITraceSink
{
    public TraceSinkId TraceSinkId => TraceSinkId.NoteType;

    public void AddTrace(TraceData traceData)
    {
    }
}