using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Service.Admin.Tracing.Tracing.TimerSettings.UseCase;

public class ChangeSnapshotSaveIntervalTraceCollector(ITracingDataSender sender)
    : IChangeSnapshotSaveIntervalTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId)
    {
        const string log = "Change active status requested";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.TimerSettings,
            UseCaseMeta.ChangeSnapshotSaveInterval,
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
            UseCaseMeta.ChangeSnapshotSaveInterval,
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
            UseCaseMeta.ChangeSnapshotSaveInterval,
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
            UseCaseMeta.ChangeSnapshotSaveInterval,
            LoggingMeta.PropertyChanged,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}