using Service.Monitoring.Enums;

namespace Service.Monitoring.Tracers.Ticket;

public class TicketTraceSink() : ITraceSink<TicketTraceRecord>
{
    private readonly Dictionary<Guid, TicketUseCaseVerifier> _useCaseVerifiers = new();

    public void AddTrace(DateTimeOffset timeStamp, LoggingMeta loggingMeta, Guid traceId, string useCaseMeta,
        Type originClassType, string log)
    {

    }
}