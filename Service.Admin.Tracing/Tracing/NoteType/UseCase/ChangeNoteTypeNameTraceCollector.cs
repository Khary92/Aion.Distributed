using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Service.Admin.Tracing.Tracing.NoteType.UseCase;

public class ChangeNoteTypeNameTraceCollector(ITracingDataSender sender)
    : IChangeNoteTypeNameTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId)
    {
        const string log = "Change Name requested";
        await sender.Send(new ServiceTraceDataCommand(
            SortingType.NoteType,
            UseCaseMeta.ChangeNoteTypeName,
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
            SortingType.NoteType,
            UseCaseMeta.ChangeNoteTypeName,
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
            SortingType.NoteType,
            UseCaseMeta.ChangeNoteTypeName,
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
            SortingType.NoteType,
            UseCaseMeta.ChangeNoteTypeName,
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
            SortingType.NoteType,
            UseCaseMeta.ChangeNoteTypeName,
            LoggingMeta.PropertyChanged,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}