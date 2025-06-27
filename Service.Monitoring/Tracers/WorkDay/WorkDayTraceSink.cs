using System.Collections.ObjectModel;
using Service.Monitoring.Enums;

namespace Service.Monitoring.Tracers.WorkDay;

public class WorkDayTraceSink : ITraceSink<WorkDayTraceRecord>
{
    private readonly Collection<WorkDayTraceRecord> _workDayTraces = [];

    public Collection<WorkDayTraceRecord> GetTraces()
    {
        return _workDayTraces;
    }

    public void AddTrace(DateTimeOffset timeStamp, LoggingMeta loggingMeta, Guid traceId, string useCaseMeta,
        Type originClassType, string log)
    {
        _workDayTraces.Add(new WorkDayTraceRecord(timeStamp, loggingMeta, traceId, useCaseMeta,
            originClassType, log));
    }
}