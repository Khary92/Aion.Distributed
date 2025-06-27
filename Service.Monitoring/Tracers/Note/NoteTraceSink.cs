using Client.Desktop.Proto.Tracing.Enums;
using Proto.Command.TraceData;

namespace Service.Monitoring.Tracers.Note;

public class NoteTraceSink : ITraceSink
{
    public TraceSinkId TraceSinkId => TraceSinkId.Note;

    public void AddTrace(TraceDataCommandProto traceDataCommandProto)
    {
    }
}