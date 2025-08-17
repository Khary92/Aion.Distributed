using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Service.Admin.Tracing.Tracing.TimerSettings.UseCase;

public class CreateTimerSettingsTraceCollector(ITracingDataSender sender)
    : ICreateTimerSettingsTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId)
    {
        var log = "Create Sprint requested";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.TimerSettings,
            UseCaseMeta.CreateTimerSettings,
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
            SortingType.TimerSettings,
            UseCaseMeta.CreateTimerSettings,
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
            SortingType.TimerSettings,
            UseCaseMeta.CreateTimerSettings,
            LoggingMeta.NotificationReceived,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task AggregateReceived(Type originClassType, Guid traceId, string attributes)
    {
        var log = $"Received aggregate {attributes}";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.TimerSettings,
            UseCaseMeta.CreateTimerSettings,
            LoggingMeta.AggregateReceived,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task AggregateAdded(Type originClassType, Guid traceId)
    {
        var log = $"Added aggregate with id:{traceId}";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.TimerSettings,
            UseCaseMeta.CreateTimerSettings,
            LoggingMeta.AggregateAdded,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}