using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Service.Admin.Tracing.Tracing.Sprint.UseCase;

public class UpdateSprintCollector(ITracingDataCommandSender commandSender) : IUpdateSprintCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId)
    {
        var log = "Change sprint data requested";

        await commandSender.Send(new ServiceTraceDataCommand(
            SortingType.Sprint,
            UseCaseMeta.UpdateSprint,
            LoggingMeta.ActionRequested,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task CommandSent(Type originClassType, Guid traceId, object command)
    {
        var log = $"Sent {command}";

        await commandSender.Send(new ServiceTraceDataCommand(
            SortingType.Sprint,
            UseCaseMeta.UpdateSprint,
            LoggingMeta.SendingCommand,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task NotificationReceived(Type originClassType, Guid traceId, object notification)
    {
        var log = $"Received {notification}";

        await commandSender.Send(new ServiceTraceDataCommand(
            SortingType.Sprint,
            UseCaseMeta.UpdateSprint,
            LoggingMeta.NotificationReceived,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task NoAggregateFound(Type originClassType, Guid traceId)
    {
        var log = $"Aggregate not found id:{traceId}";

        await commandSender.Send(new ServiceTraceDataCommand(
            SortingType.Sprint,
            UseCaseMeta.UpdateSprint,
            LoggingMeta.AggregateNotFound,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task ChangesApplied(Type originClassType, Guid traceId)
    {
        var log = $"Changed applied id:{traceId}";

        await commandSender.Send(new ServiceTraceDataCommand(
            SortingType.Sprint,
            UseCaseMeta.UpdateSprint,
            LoggingMeta.PropertyChanged,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task NoEntitySelected(Type originClassType, Guid traceId)
    {
        var log = "No ticket entity selected";
        await commandSender.Send(new ServiceTraceDataCommand(
            SortingType.Sprint,
            UseCaseMeta.UpdateSprint,
            LoggingMeta.NoEntitySelected,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}