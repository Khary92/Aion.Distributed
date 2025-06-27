using Client.Desktop.Proto.Tracing.Enums;
using Proto.Command.TraceData;

namespace Service.Monitoring.Tracers.Export;

public class ExportTraceSink : ITraceSink
{
    public TraceSinkId TraceSinkId => TraceSinkId.Export;

    public void AddTrace(TraceDataCommandProto traceDataCommandProto)
    {
    }
}