using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Client.Tracing.Tracing.Tracers.Note.UseCase;

public class CreateNoteTraceCollector(ITracingDataCommandSender commandSender) : ICreateNoteTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId)
    {
        var log = "Note creation requested";
        await commandSender.Send(new ServiceTraceDataCommand(
            SortingType.Note,
            UseCaseMeta.CreateNote,
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
            SortingType.Note,
            UseCaseMeta.CreateNote,
            LoggingMeta.SendingCommand,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task AggregateReceived(Type originClassType, Guid traceId, string attributes)
    {
        var log = $"Received aggregate {attributes}";
        await commandSender.Send(new ServiceTraceDataCommand(
            SortingType.Note,
            UseCaseMeta.CreateNote,
            LoggingMeta.AggregateReceived,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task AggregateAdded(Type originClassType, Guid traceId)
    {
        var log = $"Added aggregate with id:{traceId}";
        await commandSender.Send(new ServiceTraceDataCommand(
            SortingType.Note,
            UseCaseMeta.CreateNote,
            LoggingMeta.AggregateAdded,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task ExceptionOccured(Type originClassType, Guid traceId, Exception exception)
    {
        var log = $"Exception occured {exception}";
        await commandSender.Send(new ServiceTraceDataCommand(
            SortingType.Note,
            UseCaseMeta.CreateNote,
            LoggingMeta.ExceptionOccured,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}