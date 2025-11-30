using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Service.Admin.Tracing.Tracing.Ticket.UseCase;

public class ChangeDocumentationTraceCollector(ITracingDataSender sender) : IChangeDocumentationTraceCollector
{
    public async Task NotificationReceived(Type originClassType, Guid traceId, object notification)
    {
        var log = $"Received {notification}";
        await sender.Send(new ServiceTraceDataCommand(
            SortingType.Ticket,
            UseCaseMeta.UpdateTicketDocumentation,
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
            SortingType.Ticket,
            UseCaseMeta.UpdateTicketDocumentation,
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
            SortingType.Ticket,
            UseCaseMeta.UpdateTicketDocumentation,
            LoggingMeta.PropertyChanged,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}