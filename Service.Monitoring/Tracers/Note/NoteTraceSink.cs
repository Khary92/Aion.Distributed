using System.Collections.ObjectModel;
using Service.Monitoring.Enums;

namespace Service.Monitoring.Tracers.Note;

public class NoteTraceSink : ITraceSink<NoteTraceRecord>
{
    private readonly Collection<NoteTraceRecord> _noteTraces = [];
    public Collection<NoteTraceRecord> GetTraces()
    {
        return _noteTraces;
    }

    public void AddTrace(DateTimeOffset timeStamp, LoggingMeta loggingMeta, Guid traceId, string useCaseMeta,
        Type originClassType, string log)
    {
        _noteTraces.Add(new NoteTraceRecord(timeStamp, loggingMeta, traceId, useCaseMeta,
            originClassType, log));
    }
}