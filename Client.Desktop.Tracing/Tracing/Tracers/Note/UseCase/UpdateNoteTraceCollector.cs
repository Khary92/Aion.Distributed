using Core.Server.Tracing.Communication.Tracing;
using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Client.Desktop.Tracing.Tracing.Tracers.Note.UseCase;

public class UpdateNoteTraceCollector(ITracingDataCommandSender commandSender) : IUpdateNoteTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes)
    {
        var log = $"Update Note requested {attributes}";
        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.Note,
            UseCaseMeta.UpdateNote,
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
            TraceSinkId.Note,
            UseCaseMeta.UpdateNote,
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
            TraceSinkId.Note,
            UseCaseMeta.UpdateNote,
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
            TraceSinkId.Note,
            UseCaseMeta.UpdateNote,
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
            TraceSinkId.Note,
            UseCaseMeta.UpdateNote,
            LoggingMeta.ActionRequested,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task ExceptionOccured(Type originClassType, Guid traceId, Exception exception)
    {
        var log = $"Exception occured {exception}";
        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.Note,
            UseCaseMeta.UpdateNote,
            LoggingMeta.ExceptionOccured,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}