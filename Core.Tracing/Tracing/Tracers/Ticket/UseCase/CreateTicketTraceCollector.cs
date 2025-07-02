using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Enums;
using Service.Monitoring.Shared.Tracing;

namespace Core.Server.Tracing.Tracing.Tracers.Ticket.UseCase;

public class CreateTicketTraceCollector(ITracingDataCommandSender commandSender) : ICreateTicketTraceCollector
{
    public async Task CommandReceived(Type originClassType, Guid traceId, object protoCommand)
    {
        var log = ($"Command received {protoCommand.GetType()}:{protoCommand}");
        
        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.Ticket,
            UseCaseMeta.CreateTicket,
            LoggingMeta.CommandReceived,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
    
    public async Task EventPersisted(Type originClassType, Guid traceId, object @event)
    {
        var log = ($"Notification sent {@event.GetType()}:{@event}");
        
        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.Ticket,
            UseCaseMeta.CreateTicket,
            LoggingMeta.EventPersisted,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }

    public async Task NotificationSent(Type originClassType, Guid traceId, object notification)
    {
        var log = ($"Notification sent {notification.GetType()}:{notification}");
        
        await commandSender.Send(new ServiceTraceDataCommand(
            TraceSinkId.Ticket,
            UseCaseMeta.CreateTicket,
            LoggingMeta.CommandReceived,
            originClassType,
            traceId,
            log,
            DateTimeOffset.Now));
    }
}