using Proto.Command.TraceData;
using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Service.Monitoring.Tracers.Note;

public class NoteTraceSink : ITraceSink
{
    public TraceSinkId TraceSinkId => TraceSinkId.Note;

    public void AddTrace(TraceData traceData)
    {
    }
}