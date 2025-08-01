using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Core.Server.Tracing.Tracing.Tracers.NoteType.UseCase;

public class ChangeNoteTypeColorTraceCollector(ITracingDataCommandSender commandSender)
    : IChangeNoteTypeColorTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes)
    {
        var log = $"Change Color requested for {attributes}";
        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.NoteType,
            UseCaseMeta.ChangeNoteTypeColor,
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
            TraceSinkId.NoteType,
            UseCaseMeta.ChangeNoteTypeColor,
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
            TraceSinkId.NoteType,
            UseCaseMeta.ChangeNoteTypeColor,
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
            TraceSinkId.NoteType,
            UseCaseMeta.ChangeNoteTypeColor,
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
            TraceSinkId.NoteType,
            UseCaseMeta.ChangeNoteTypeColor,
            LoggingMeta.PropertyChanged,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}