using System.Collections.ObjectModel;
using Service.Monitoring.Enums;

namespace Service.Monitoring.Tracers.Export;

public class ExportTraceSink : ITraceSink<ExportTraceRecord>
{
    private readonly Collection<ExportTraceRecord> _exportTraces = [];
    public Collection<ExportTraceRecord> GetTraces()
    {
        return _exportTraces;
    }

    public void AddTrace(DateTimeOffset timeStamp, LoggingMeta loggingMeta, Guid traceId, string useCaseMeta,
        Type originClassType, string log)
    {
        // TraceId is lost here. There is no traceable id
        _exportTraces.Add(new ExportTraceRecord(timeStamp, loggingMeta, useCaseMeta, originClassType, log));
    }
}