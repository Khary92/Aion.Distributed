using Proto.Command.TraceData;
using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Service.Monitoring.Tracers.Export;

public class ExportTraceSink : ITraceSink
{
    public TraceSinkId TraceSinkId => TraceSinkId.Export;

    public void AddTrace(TraceData traceData)
    {
    }
}