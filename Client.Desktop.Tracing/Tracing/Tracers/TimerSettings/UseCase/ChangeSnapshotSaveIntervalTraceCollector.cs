using Client.Desktop.Tracing.Communication.Tracing;
using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Client.Desktop.Tracing.Tracing.Tracers.TimerSettings.UseCase;

public class ChangeSnapshotSaveIntervalTraceCollector(ITracingDataCommandSender commandSender) : IChangeSnapshotSaveIntervalTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes)
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

    public async Task CommandSent(Type originClassType, Guid traceId, object command)
    {
        var log = ($"Sent {command}");
        
        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.TimerSettings,
            UseCaseMeta.ChangeSnapshotSaveInterval,
            LoggingMeta.CommandSent,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task NotificationReceived(Type originClassType, Guid traceId, object notification)
    {
        var log = ($"Received {notification}");
        
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
        var log = ($"Aggregate not found id:{traceId}");
        
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
        var log = ($"Changed applied id:{traceId}");
        
        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.TimerSettings,
            UseCaseMeta.ChangeSnapshotSaveInterval,
            LoggingMeta.PropertyChanged,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task PropertyNotChanged(Type originClassType, Guid traceId, Dictionary<string, string> asTraceAttributes)
    {
        var log = ($"Request aborted {asTraceAttributes}");
    
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