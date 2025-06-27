using System.Collections.ObjectModel;
using Service.Monitoring.Enums;

namespace Service.Monitoring.Tracers.Sprint;

public class SprintTraceSink: ITraceSink<SprintTraceRecord>
{
    private readonly Collection<SprintTraceRecord> _noteTypeTraces = [];
    public Collection<SprintTraceRecord> GetTraces()
    {
        return _noteTypeTraces;
    }

    public void AddTrace(DateTimeOffset timeStamp, LoggingMeta loggingMeta, Guid traceId, string useCaseMeta,
        Type originClassType, string log)
    {
        _noteTypeTraces.Add(new SprintTraceRecord(timeStamp, loggingMeta, traceId, useCaseMeta,
            originClassType, log));
    }
}