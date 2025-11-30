using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Service.Admin.Tracing.Tracing.Sprint.UseCase;

public class CreateSprintTraceCollector(ITracingDataSender sender) : ICreateSprintTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId)
    {
        const string log = "Create Sprint requested";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.Sprint,
            UseCaseMeta.CreateSprint,
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
            SortingType.Sprint,
            UseCaseMeta.CreateSprint,
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
            SortingType.Sprint,
            UseCaseMeta.CreateSprint,
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
            SortingType.Sprint,
            UseCaseMeta.CreateSprint,
            LoggingMeta.AggregateAdded,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}