using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Client.Tracing.Tracing.Tracers.Tag.UseCase;

public class CreateTagTraceCollector(ITracingDataSender sender) : ICreateTagTraceCollector
{
    public async Task NotificationReceived(Type originClassType, Guid traceId, object notification)
    {
        var log = $"Notification received {notification}";
        await sender.Send(new ServiceTraceDataCommand(
            SortingType.Tag,
            UseCaseMeta.CreateTag,
            LoggingMeta.NotificationReceived,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task AggregateAdded(Type originClassType, Guid traceId)
    {
        var log = $"Added aggregate with id:{traceId}";
        await sender.Send(new ServiceTraceDataCommand(
            SortingType.Tag,
            UseCaseMeta.CreateTag,
            LoggingMeta.AggregateAdded,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}