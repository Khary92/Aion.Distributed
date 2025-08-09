using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Service.Admin.Tracing.Tracing.Ticket.UseCase;

public class UpdateTicketTraceCollector(ITracingDataCommandSender commandSender) : IUpdateTicketTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId)
    {
        var log = "Update ticket requested";

        await commandSender.Send(new ServiceTraceDataCommand(
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
        var log = "No ticket entity selected";
        await commandSender.Send(new ServiceTraceDataCommand(
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

        await commandSender.Send(new ServiceTraceDataCommand(
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
        await commandSender.Send(new ServiceTraceDataCommand(
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
        await commandSender.Send(new ServiceTraceDataCommand(
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
        await commandSender.Send(new ServiceTraceDataCommand(
            SortingType.Ticket,
            UseCaseMeta.UpdateTicket,
            LoggingMeta.PropertyChanged,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}