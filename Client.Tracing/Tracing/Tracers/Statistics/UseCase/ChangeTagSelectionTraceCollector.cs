using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Client.Tracing.Tracing.Tracers.Statistics.UseCase;

public class ChangeTagSelectionTraceCollector(ITracingDataSender sender)
    : IChangeTagSelectionTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId)
    {
        const string log = "Change statistics data requested";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.StatisticsData,
            UseCaseMeta.ChangeTagSelection,
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
            SortingType.StatisticsData,
            UseCaseMeta.ChangeTagSelection,
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
            SortingType.StatisticsData,
            UseCaseMeta.ChangeTagSelection,
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
            SortingType.StatisticsData,
            UseCaseMeta.ChangeTagSelection,
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
            SortingType.StatisticsData,
            UseCaseMeta.ChangeTagSelection,
            LoggingMeta.PropertyChanged,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task WrongModel(Type originClassType, Guid traceId)
    {
        throw new NotImplementedException();
    }
}