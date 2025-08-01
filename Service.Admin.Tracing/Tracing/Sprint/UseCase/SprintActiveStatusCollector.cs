using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Service.Admin.Tracing.Tracing.Sprint.UseCase;

public class SprintActiveStatusCollector(ITracingDataCommandSender commandSender) : ISprintActiveStatusCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId)
    {
        var log = $"Change active status requested";

        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.Sprint,
            UseCaseMeta.ChangeSprintActiveStatus,
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
            TraceSinkId.Sprint,
            UseCaseMeta.ChangeSprintActiveStatus,
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
            TraceSinkId.Sprint,
            UseCaseMeta.ChangeSprintActiveStatus,
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
            TraceSinkId.Sprint,
            UseCaseMeta.ChangeSprintActiveStatus,
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
            TraceSinkId.Sprint,
            UseCaseMeta.ChangeSprintActiveStatus,
            LoggingMeta.PropertyChanged,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task NoEntitySelected(Type originClassType, Guid traceId)
    {
        var log = $"No ticket entity selected";
        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.Sprint,
            UseCaseMeta.ChangeSprintActiveStatus,
            LoggingMeta.NoEntitySelected,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}