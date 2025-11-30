using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Client.Tracing.Tracing.Tracers.Ticket.UseCase;

public class UpdateTicketTraceCollector(ITracingDataSender sender) : IUpdateTicketTraceCollector
{
    private const SortingType Sorting = SortingType.Ticket;
    private const UseCaseMeta UseCase = UseCaseMeta.UpdateTicket;

    public async Task NotificationReceived(Type originClassType, Guid traceId, object notification)
    {
        var log = $"Received {notification}";
        await sender.Send(new ServiceTraceDataCommand(
            Sorting,
            UseCase,
            LoggingMeta.NotificationReceived,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task NoAggregateFound(Type originClassType, Guid traceId)
    {
        var log = $"Aggregate not found id:{traceId}";
        await sender.Send(new ServiceTraceDataCommand(
            Sorting,
            UseCase,
            LoggingMeta.AggregateNotFound,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task ChangesApplied(Type originClassType, Guid traceId)
    {
        var log = $"Changed applied id:{traceId}";
        await sender.Send(new ServiceTraceDataCommand(
            Sorting,
            UseCase,
            LoggingMeta.PropertyChanged,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}