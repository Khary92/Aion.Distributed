using System.Collections.ObjectModel;
using Service.Monitoring.Enums;

namespace Service.Monitoring.Tracers.Tag;

public class TagTraceSink : ITraceSink<TagTraceRecord>
{
    private readonly Collection<TagTraceRecord> _tagTraces = [];
    public Collection<TagTraceRecord> GetTraces()
    {
        return _tagTraces;
    }

    public void AddTrace(DateTimeOffset timeStamp, LoggingMeta loggingMeta, Guid traceId, string useCaseMeta,
        Type originClassType, string log)
    {
        var tagTraceRecord = new TagTraceRecord(timeStamp, loggingMeta, traceId, useCaseMeta,
            originClassType, log);
        _tagTraces.Add(tagTraceRecord);
        
    }
}