using Core.Server.Communication.Tracing;
using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;

namespace Core.Server.Tracing.Tracing.Tracers.Ticket.UseCase;

public class UpdateTicketTraceCollector(ITracingDataSender sender) : IUpdateTicketTraceCollector
{
    public async Task CommandReceived(Type originClassType, Guid traceId, object protoCommand)
    {
        var log = $"Command received {GetName(protoCommand)}:{protoCommand}";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.Ticket,
            UseCaseMeta.UpdateTicket,
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
            SortingType.Ticket,
            UseCaseMeta.UpdateTicket,
            LoggingMeta.EventPersisted,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task NotificationSent(Type originClassType, Guid traceId, object notification)
    {
        var log = $"Notification sent {GetName(notification)}:{notification}";

        await sender.Send(new ServiceTraceDataCommand(
            SortingType.Ticket,
            UseCaseMeta.UpdateTicket,
            LoggingMeta.SendingNotification,
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