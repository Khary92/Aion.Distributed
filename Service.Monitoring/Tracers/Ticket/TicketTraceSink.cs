using Service.Monitoring.Enums;

namespace Service.Monitoring.Tracers.Ticket;

public class TicketTraceSink(IReportDispatcher reportDispatcher) : ITraceSink<TicketTraceRecord>
{
    private readonly Dictionary<Guid, TicketUseCaseVerifier> _useCaseVerifiers = new();

    public void AddTrace(DateTimeOffset timeStamp, LoggingMeta loggingMeta, Guid traceId, string useCaseMeta,
        Type originClassType, string log)
    {
        if (!_useCaseVerifiers.ContainsKey(traceId) && loggingMeta == LoggingMeta.ActionRequested)
        {
            _useCaseVerifiers[traceId] = new TicketUseCaseVerifier(reportDispatcher);
        }

        _useCaseVerifiers[traceId].ProcessAsync(new TicketTraceRecord(timeStamp, loggingMeta, traceId,
            TicketUseCase.AddTicketToCurrentSprint,
            originClassType, log)).ConfigureAwait(true);
    }
}