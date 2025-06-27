using Client.Desktop.Proto.Tracing.Enums;
using Proto.Command.TraceData;

namespace Service.Monitoring.Tracers.NoteType;

public class NoteTypeTraceSink : ITraceSink
{
    public TraceSinkId TraceSinkId => TraceSinkId.NoteType;

    public void AddTrace(TraceDataCommandProto traceDataCommandProto)
    {
    }
}