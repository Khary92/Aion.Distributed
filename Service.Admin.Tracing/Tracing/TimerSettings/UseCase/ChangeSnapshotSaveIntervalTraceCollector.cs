using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Service.Admin.Tracing.Tracing.TimerSettings.UseCase;

public class ChangeSnapshotSaveIntervalTraceCollector(ITracingDataCommandSender commandSender)
    : IChangeSnapshotSaveIntervalTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId, string attributes)
    {
        var log = $"Change active status requested for {attributes}";

        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.TimerSettings,
            UseCaseMeta.ChangeSnapshotSaveInterval,
            LoggingMeta.ActionRequested,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task SendingCommand(Type originClassType, Guid traceId, object command)
    {
        var log = $"Sent {command}";

        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.TimerSettings,
            UseCaseMeta.ChangeSnapshotSaveInterval,
            LoggingMeta.SendingCommand,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task NotificationReceived(Type originClassType, Guid traceId, object notification)
    {
        var log = $"Received {notification}";

        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.TimerSettings,
            UseCaseMeta.ChangeSnapshotSaveInterval,
            LoggingMeta.NotificationReceived,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task NoAggregateFound(Type originClassType, Guid traceId)
    {
        var log = $"Aggregate not found id:{traceId}";

        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.TimerSettings,
            UseCaseMeta.ChangeSnapshotSaveInterval,
            LoggingMeta.AggregateNotFound,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task ChangesApplied(Type originClassType, Guid traceId)
    {
        var log = $"Changed applied id:{traceId}";

        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.TimerSettings,
            UseCaseMeta.ChangeSnapshotSaveInterval,
            LoggingMeta.PropertyChanged,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task PropertyNotChanged(Type originClassType, Guid traceId, string asTraceAttributes)
    {
        var log = $"Request aborted {asTraceAttributes}";

        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.TimerSettings,
            UseCaseMeta.ChangeSnapshotSaveInterval,
            LoggingMeta.PropertyNotChanged,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}