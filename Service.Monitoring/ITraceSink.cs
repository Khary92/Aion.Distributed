using Service.Monitoring.Enums;

namespace Service.Monitoring;

public interface ITraceSink<TTraceRecord>
{
    void AddTrace(DateTimeOffset timeStamp, LoggingMeta loggingMeta, Guid traceId, string useCaseMeta,
        Type originClassType, string log);
}