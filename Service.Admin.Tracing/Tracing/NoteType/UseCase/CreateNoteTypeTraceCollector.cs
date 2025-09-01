using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Service.Admin.Tracing.Tracing.NoteType.UseCase;

public class CreateNoteTypeTraceCollector(ITracingDataSender sender) : ICreateNoteTypeTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId, string attributes)
    {
        var log = $"Create NoteType requested for {attributes}";
        await sender.Send(new ServiceTraceDataCommand(
            SortingType.NoteType,
            UseCaseMeta.CreateNoteType,
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
            UseCaseMeta.CreateNoteType,
            LoggingMeta.SendingCommand,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task NotificationReceived(Type originClassType, Guid traceId, object notification)
    {
        var log = $"Received {GetName(notification)}:{notification}";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.NoteType,
            UseCaseMeta.CreateNoteType,
            LoggingMeta.NotificationReceived,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
    
    public async Task AggregateAdded(Type originClassType, Guid traceId)
    {
        var log = $"Added aggregate with id:{traceId}";
        await sender.Send(new ServiceTraceDataCommand(
            SortingType.NoteType,
            UseCaseMeta.CreateNoteType,
            LoggingMeta.AggregateAdded,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    private static string GetName(object @object)
    {
        var commandType = @object.GetType();
        return commandType.Name;
    }
}