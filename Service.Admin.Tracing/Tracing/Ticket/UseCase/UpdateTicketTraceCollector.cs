using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Service.Admin.Tracing.Tracing.Ticket.UseCase;

public class UpdateTicketTraceCollector(ITracingDataSender sender) : IUpdateTicketTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId)
    {
        const string log = "Update ticket requested";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.Ticket,
            UseCaseMeta.UpdateTicket,
            LoggingMeta.ActionRequested,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task NoEntitySelected(Type originClassType, Guid traceId)
    {
        const string log = "No ticket entity selected";
        await sender.Send(new ServiceTraceDataCommand(
            SortingType.Ticket,
            UseCaseMeta.AddTicketToCurrentSprint,
            LoggingMeta.NoEntitySelected,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task SendingCommand(Type originClassType, Guid traceId, object command)
    {
        var log = $"Sent {command}";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.Ticket,
            UseCaseMeta.UpdateTicket,
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
            SortingType.Ticket,
            UseCaseMeta.UpdateTicket,
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
            SortingType.Ticket,
            UseCaseMeta.UpdateTicket,
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
            SortingType.Ticket,
            UseCaseMeta.UpdateTicket,
            LoggingMeta.PropertyChanged,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}