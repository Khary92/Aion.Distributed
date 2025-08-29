using Service.Monitoring.Shared;

namespace Service.Monitoring.Sink;

public interface ITraceSink
{
    void AddTrace(TraceData traceData);
}