using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Service.Admin.Tracing.Tracing.Tag.UseCase;

public class UpdateTagTraceCollector(ITracingDataCommandSender commandSender) : IUpdateTagTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId, string attributes)
    {
        var log = $"Change tag data requested for {attributes}";

        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.Tag,
            UseCaseMeta.UpdateTag,
            LoggingMeta.ActionRequested,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task CommandSent(Type originClassType, Guid traceId, object command)
    {
        var log = $"Sent {command}";
        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.Tag,
            UseCaseMeta.UpdateTag,
            LoggingMeta.CommandSent,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task NotificationReceived(Type originClassType, Guid traceId, object notification)
    {
        var log = $"Received {notification}";
        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.Tag,
            UseCaseMeta.UpdateTag,
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
            TraceSinkId.Tag,
            UseCaseMeta.UpdateTag,
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
            TraceSinkId.Tag,
            UseCaseMeta.UpdateTag,
            LoggingMeta.PropertyChanged,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}