using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Client.Tracing.Tracing.Tracers.Statistics.UseCase;

public class ChangeProductivityTraceCollector(ITracingDataSender sender)
    : IChangeProductivityTraceCollector
{
    private const SortingType Sorting = SortingType.StatisticsData;
    private const UseCaseMeta UseCase = UseCaseMeta.ChangeProductivity;

    public async Task StartUseCase(Type originClassType, Guid traceId)
    {
        const string log = "Change statics data requested";

        await sender.Send(new ServiceTraceDataCommand(
            Sorting,
            UseCase,
            LoggingMeta.ActionRequested,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task SendingCommand(Type originClassType, Guid traceId, object command)
    {
        var log = $"Sent {command}";

        await sender.Send(new ServiceTraceDataCommand(
            Sorting,
            UseCase,
            LoggingMeta.SendingCommand,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

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