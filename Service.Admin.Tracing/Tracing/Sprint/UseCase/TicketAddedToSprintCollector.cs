using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Service.Admin.Tracing.Tracing.Sprint.UseCase;

public class TicketAddedToSprintCollector(ITracingDataSender sender) : ITicketAddedToSprintCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId)
    {
        const string log = "Add ticket to sprint requested";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.Sprint,
            UseCaseMeta.AddTicketToCurrentSprint,
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
            UseCaseMeta.AddTicketToCurrentSprint,
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
            UseCaseMeta.AddTicketToCurrentSprint,
            LoggingMeta.NotificationReceived,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task NoAggregateFound(Type originClassType, Guid traceId)
    {
        var log = $"Aggregate not found id:{traceId}";
        await sender.Send(new ServiceTraceDataCommand(
            SortingType.Sprint,
            UseCaseMeta.AddTicketToCurrentSprint,
            LoggingMeta.AggregateNotFound,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task ChangesApplied(Type originClassType, Guid traceId)
    {
        var log = $"Changed applied id:{traceId}";
        await sender.Send(new ServiceTraceDataCommand(
            SortingType.Sprint,
            UseCaseMeta.AddTicketToCurrentSprint,
            LoggingMeta.PropertyChanged,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task NoEntitySelected(Type originClassType, Guid traceId)
    {
        const string log = "No ticket entity selected";
        await sender.Send(new ServiceTraceDataCommand(
            SortingType.Sprint,
            UseCaseMeta.AddTicketToCurrentSprint,
            LoggingMeta.NoEntitySelected,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}