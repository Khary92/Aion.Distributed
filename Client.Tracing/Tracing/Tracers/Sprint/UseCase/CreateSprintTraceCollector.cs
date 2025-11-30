using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Client.Tracing.Tracing.Tracers.Sprint.UseCase;

public class CreateSprintTraceCollector(ITracingDataSender sender) : ICreateSprintTraceCollector
{
    private const SortingType Sorting = SortingType.Sprint;
    private const UseCaseMeta UseCase = UseCaseMeta.CreateSprint;

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