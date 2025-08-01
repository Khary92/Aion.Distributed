using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Service.Admin.Tracing.Tracing.Sprint.UseCase;

public class TicketAddedToSprintCollector(ITracingDataCommandSender commandSender) : ITicketAddedToSprintCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId)
    {
        var log = $"Add ticket to sprint requested";

        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.Sprint,
            UseCaseMeta.TicketAddedToSprint,
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
            UseCaseMeta.TicketAddedToSprint,
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
            UseCaseMeta.TicketAddedToSprint,
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
            UseCaseMeta.TicketAddedToSprint,
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
            UseCaseMeta.TicketAddedToSprint,
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
            UseCaseMeta.AddTicketToCurrentSprint,
            LoggingMeta.NoEntitySelected,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}