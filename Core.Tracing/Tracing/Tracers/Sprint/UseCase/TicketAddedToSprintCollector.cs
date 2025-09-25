using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Core.Server.Tracing.Tracing.Tracers.Sprint.UseCase;

public class TicketAddedToSprintCollector(ITracingDataSender sender) : ITicketAddedToSprintCollector
{
    public async Task CommandReceived(Type originClassType, Guid traceId, object protoCommand)
    {
        var log = $"Command received {GetName(protoCommand)}:{protoCommand}";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.Sprint,
            UseCaseMeta.AddTicketToCurrentSprint,
            LoggingMeta.CommandReceived,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task EventPersisted(Type originClassType, Guid traceId, object @event)
    {
        var log = $"Event persisted {@event}";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.Sprint,
            UseCaseMeta.AddTicketToCurrentSprint,
            LoggingMeta.EventPersisted,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task SendingNotification(Type originClassType, Guid traceId, object notification)
    {
        var log = $"Notification sent {GetName(notification)}:{notification}";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.Sprint,
            UseCaseMeta.AddTicketToCurrentSprint,
            LoggingMeta.SendingNotification,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task TicketNotFound(Type originClassType, Guid traceId)
    {
        var log = $"Ticket with id {traceId} not found";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.Sprint,
            UseCaseMeta.AddTicketToCurrentSprint,
            LoggingMeta.AggregateNotFound,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task NoActiveSprint(Type originClassType, Guid traceId)
    {
        var log = "No active sprint found";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.Sprint,
            UseCaseMeta.AddTicketToCurrentSprint,
            LoggingMeta.AggregateNotFound,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task AddedSprintIdToTicket(Type originClassType, Guid traceId, Guid activeSprintSprintId)
    {
        var log = $"Added sprint id {activeSprintSprintId} to ticket";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.Sprint,
            UseCaseMeta.AddTicketToCurrentSprint,
            LoggingMeta.SprintIdAddedToTicket,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task TicketIsAlreadyInSprint(Type originClassType, Guid traceId)
    {
        var log = "Ticket was already added to sprint";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.Sprint,
            UseCaseMeta.AddTicketToCurrentSprint,
            LoggingMeta.ActionAborted,
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