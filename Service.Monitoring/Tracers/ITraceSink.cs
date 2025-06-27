using Client.Desktop.Proto.Tracing.Enums;
using Proto.Command.TraceData;

namespace Service.Monitoring.Tracers;

public interface ITraceSink
{
    TraceSinkId TraceSinkId { get; }
    void AddTrace(TraceDataCommandProto traceDataCommandProto);
}