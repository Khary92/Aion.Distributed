using Client.Desktop.Proto.Tracing.Enums;
using Client.Desktop.Tracing.Communication.Tracing;

namespace Client.Desktop.Tracing.Tracing.Tracers.NoteType.UseCase;

public class ChangeNoteTypeNameTraceCollector(ITracingDataCommandSender commandSender)
    : IChangeNoteTypeNameTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes)
    {
        var log = $"Change Name requested for {attributes}";
        await commandSender.Send(new TraceDataCommand(
            TraceSinkId.NoteType,
            UseCaseMeta.ChangeNoteTypeName,
            LoggingMeta.ActionRequested,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task CommandSent(Type originClassType, Guid traceId, object command)
    {
        var log = ($"Sent {command}");
        await commandSender.Send(new TraceDataCommand(
            TraceSinkId.NoteType,
            UseCaseMeta.ChangeNoteTypeName,
            LoggingMeta.CommandSent,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task NotificationReceived(Type originClassType, Guid traceId, object notification)
    {
        var log = ($"Received {notification}");
        await commandSender.Send(new TraceDataCommand(
            TraceSinkId.NoteType,
            UseCaseMeta.ChangeNoteTypeName,
            LoggingMeta.NotificationReceived,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task NoAggregateFound(Type originClassType, Guid traceId)
    {
        var log = ($"Aggregate not found id:{traceId}");
        await commandSender.Send(new TraceDataCommand(
            TraceSinkId.NoteType,
            UseCaseMeta.ChangeNoteTypeName,
            LoggingMeta.AggregateNotFound,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task ChangesApplied(Type originClassType, Guid traceId)
    {
        var log = ($"Changed applied id:{traceId}");
        await commandSender.Send(new TraceDataCommand(
            TraceSinkId.NoteType,
            UseCaseMeta.ChangeNoteTypeName,
            LoggingMeta.PropertyChanged,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}