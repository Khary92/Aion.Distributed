using System.Collections.ObjectModel;
using Service.Monitoring.Enums;

namespace Service.Monitoring.Tracers.TimerSettings;

public class TimerSettingsTraceSink : ITraceSink<TimerSettingsTraceRecord>
{
    private readonly Collection<TimerSettingsTraceRecord> _timerSettingsTraces = [];

    public Collection<TimerSettingsTraceRecord> GetTraces()
    {
        return _timerSettingsTraces;
    }

    public void AddTrace(DateTimeOffset timeStamp, LoggingMeta loggingMeta, Guid traceId, string useCaseMeta,
        Type originClassType, string log)
    {
        _timerSettingsTraces.Add(new TimerSettingsTraceRecord(timeStamp, loggingMeta, traceId, useCaseMeta,
            originClassType, log));
    }
}