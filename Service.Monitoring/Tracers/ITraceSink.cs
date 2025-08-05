using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Service.Monitoring.Tracers;

public interface ITraceSink
{
    void AddTrace(TraceData traceData);
}