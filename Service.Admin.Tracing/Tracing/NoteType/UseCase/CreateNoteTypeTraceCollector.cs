using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Service.Admin.Tracing.Tracing.NoteType.UseCase;

public class CreateNoteTypeTraceCollector(ITracingDataCommandSender commandSender) : ICreateNoteTypeTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId, string attributes)
    {
        var log = $"Create NoteType requested for {attributes}";
        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.NoteType,
            UseCaseMeta.CreateNoteType,
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
            TraceSinkId.NoteType,
            UseCaseMeta.CreateNoteType,
            LoggingMeta.CommandSent,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task AggregateReceived(Type originClassType, Guid traceId, string attributes)
    {
        var log = ($"Received aggregate {attributes}");
        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.NoteType,
            UseCaseMeta.CreateNoteType,
            LoggingMeta.AggregateReceived,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task AggregateAdded(Type originClassType, Guid traceId)
    {
        var log = ($"Added aggregate with id:{traceId}");
        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.NoteType,
            UseCaseMeta.CreateNoteType,
            LoggingMeta.AggregateAdded,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}