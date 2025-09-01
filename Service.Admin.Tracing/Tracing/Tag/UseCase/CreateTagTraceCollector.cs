using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Service.Admin.Tracing.Tracing.Tag.UseCase;

public class CreateTagTraceCollector(ITracingDataSender sender) : ICreateTagTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId)
    {
        const string log = "Create Tag requested";
        await sender.Send(new ServiceTraceDataCommand(
            SortingType.Tag,
            UseCaseMeta.CreateTag,
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
            SortingType.Tag,
            UseCaseMeta.CreateTag,
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
            SortingType.Tag,
            UseCaseMeta.CreateTag,
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
            SortingType.Tag,
            UseCaseMeta.CreateTag,
            LoggingMeta.AggregateAdded,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}