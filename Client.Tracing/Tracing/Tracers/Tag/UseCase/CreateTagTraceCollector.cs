using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Client.Tracing.Tracing.Tracers.Tag.UseCase;

public class CreateTagTraceCollector(ITracingDataSender sender) : ICreateTagTraceCollector
{
    private const SortingType Sorting = SortingType.Tag;
    private const UseCaseMeta UseCase = UseCaseMeta.CreateTag;

    public async Task NotificationReceived(Type originClassType, Guid traceId, object notification)
    {
        var log = $"Notification received {notification}";
        await sender.Send(new ServiceTraceDataCommand(
            Sorting,
            UseCase,
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
            Sorting,
            UseCase,
            LoggingMeta.AggregateAdded,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}