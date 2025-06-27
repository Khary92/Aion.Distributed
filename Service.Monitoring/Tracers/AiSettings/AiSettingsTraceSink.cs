using System.Collections.ObjectModel;
using Service.Monitoring.Enums;

namespace Service.Monitoring.Tracers.AiSettings;

public class AiSettingsTraceSink : ITraceSink<AiSettingsTraceRecord>
{
    private readonly Collection<AiSettingsTraceRecord> _aiSettingsTraces = [];
    public Collection<AiSettingsTraceRecord> GetTraces()
    {
        return _aiSettingsTraces;
    }

    public void AddTrace(DateTimeOffset timeStamp, LoggingMeta loggingMeta, Guid traceId, string useCaseMeta,
        Type originClassType, string log)
    {
        _aiSettingsTraces.Add(new AiSettingsTraceRecord(timeStamp, loggingMeta, traceId, useCaseMeta,
            originClassType, log));
    }
}