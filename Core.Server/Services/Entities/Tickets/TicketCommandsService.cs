using Core.Server.Communication.Endpoints.Ticket;
using Core.Server.Communication.Records.Commands.Entities.Tickets;
using Core.Server.Tracing.Tracing.Tracers;
using Core.Server.Translators.Commands.Tickets;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.Tickets;

public class TicketCommandsService(
    ITicketEventsStore ticketEventStore,
    TicketNotificationService ticketNotificationService,
    ITicketCommandsToEventTranslator eventTranslator,
    ITraceCollector tracer)
    : ITicketCommandsService
{
    public async Task UpdateData(UpdateTicketDataCommand command)
    {
        await ticketEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        var ticketNotification = command.ToNotification();
        await tracer.Ticket.Update.EventPersisted(GetType(), command.TraceId, ticketNotification.TicketDataUpdated);

        await tracer.Ticket.Update.NotificationSent(GetType(), command.TraceId, ticketNotification.TicketDataUpdated);
        await ticketNotificationService.SendNotificationAsync(ticketNotification);
    }

    public async Task UpdateDocumentation(UpdateTicketDocumentationCommand command)
    {
        await ticketEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        
        await ticketNotificationService.SendNotificationAsync(command.ToNotification());
    }

    public async Task Create(CreateTicketCommand command)
    {
        await ticketEventStore.StoreEventAsync(eventTranslator.ToEvent(command), command.TraceId);
        var ticketNotification = command.ToNotification();
        await tracer.Ticket.Create.EventPersisted(GetType(), command.TraceId, ticketNotification.TicketCreated);

        await tracer.Ticket.Create.NotificationSent(GetType(), command.TraceId, ticketNotification.TicketCreated);
        await ticketNotificationService.SendNotificationAsync(ticketNotification);
    }
}