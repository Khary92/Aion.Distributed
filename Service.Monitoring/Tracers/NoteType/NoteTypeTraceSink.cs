using System.Collections.ObjectModel;
using Service.Monitoring.Enums;

namespace Service.Monitoring.Tracers.NoteType;

public class NoteTypeTraceSink : ITraceSink<NoteTypeTraceRecord>
{
    private readonly Collection<NoteTypeTraceRecord> _noteTypeTraces = [];
    public Collection<NoteTypeTraceRecord> GetTraces()
    {
        return _noteTypeTraces;
    }

    public void AddTrace(DateTimeOffset timeStamp, LoggingMeta loggingMeta, Guid traceId, string useCaseMeta,
        Type originClassType, string log)
    {
        _noteTypeTraces.Add(new NoteTypeTraceRecord(timeStamp, loggingMeta, traceId, useCaseMeta,
            originClassType, log));
    }
}