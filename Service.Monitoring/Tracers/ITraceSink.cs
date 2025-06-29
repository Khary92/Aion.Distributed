using Proto.Command.TraceData;
using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Service.Monitoring.Tracers;

public interface ITraceSink
{
    TraceSinkId TraceSinkId { get; }
    void AddTrace(TraceData traceData);
}