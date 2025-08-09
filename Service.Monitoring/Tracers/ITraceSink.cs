using Service.Monitoring.Shared;

namespace Service.Monitoring.Tracers;

public interface ITraceSink
{
    void AddTrace(TraceData traceData);
}