using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Service.Admin.Tracing.Tracing.Ticket.UseCase;

public class CreateTicketTraceCollector(ITracingDataCommandSender commandSender) : ICreateTicketTraceCollector
{
    public async Task StartUseCase(Type originClassType, Guid traceId)
    {
        var log = "Create Ticket requested";

        await commandSender.Send(new ServiceTraceDataCommand(
            SortingType.Ticket,
            UseCaseMeta.CreateTicket,
            LoggingMeta.ActionRequested,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task SendingCommand(Type originClassType, Guid traceId, object command)
    {
        var log = $"Sent {GetName(command)}:{command}";

        await commandSender.Send(new ServiceTraceDataCommand(
            SortingType.Ticket,
            UseCaseMeta.CreateTicket,
            LoggingMeta.SendingCommand,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task NotificationReceived(Type originClassType, Guid traceId, object notification)
    {
        var log = $"Received {GetName(notification)}:{notification}";

        await commandSender.Send(new ServiceTraceDataCommand(
            SortingType.Ticket,
            UseCaseMeta.CreateTicket,
            LoggingMeta.NotificationReceived,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task AggregateReceived(Type originClassType, Guid traceId, object command)
    {
        var log = $"Received aggregate {command}";
        await commandSender.Send(new ServiceTraceDataCommand(
            SortingType.Ticket,
            UseCaseMeta.CreateTicket,
            LoggingMeta.AggregateReceived,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task AggregateAdded(Type originClassType, Guid traceId)
    {
        var log = $"Added aggregate with id:{traceId}";
        await commandSender.Send(new ServiceTraceDataCommand(
            SortingType.Ticket,
            UseCaseMeta.CreateTicket,
            LoggingMeta.AggregateAdded,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    private static string GetName(object @object)
    {
        var commandType = @object.GetType();
        return commandType.Name;
    }
}