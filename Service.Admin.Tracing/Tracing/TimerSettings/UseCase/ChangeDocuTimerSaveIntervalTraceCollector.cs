using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Service.Admin.Tracing.Tracing.TimerSettings.UseCase;

public class ChangeDocuTimerSaveIntervalTraceCollector(ITracingDataSender sender)
    : IChangeDocuTimerSaveIntervalTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId)
    {
        const string log = "Change documentation timer save interval requested";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.TimerSettings,
            UseCaseMeta.ChangeDocuTimerSaveInterval,
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
            UseCaseMeta.ChangeDocuTimerSaveInterval,
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
            UseCaseMeta.ChangeDocuTimerSaveInterval,
            LoggingMeta.NotificationReceived,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task ChangesApplied(Type originClassType, Guid traceId)
    {
        var log = $"Changed applied id:{traceId}";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.TimerSettings,
            UseCaseMeta.ChangeDocuTimerSaveInterval,
            LoggingMeta.PropertyChanged,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}