using Core.Server.Communication.Endpoints.Ticket;
using Core.Server.Communication.Records.Commands.Entities.Tickets;
using Core.Server.Translators.Commands.Tickets;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.Tickets;

public class TicketCommandsService(
    ITicketEventsStore ticketEventStore,
    TicketNotificationService ticketNotificationService,
    ITicketCommandsToEventTranslator eventTranslator)
    : ITicketCommandsService
{
    public async Task UpdateData(UpdateTicketDataCommand updateTicketDataCommand)
    {
        await ticketEventStore.StoreEventAsync(eventTranslator.ToEvent(updateTicketDataCommand));
        await ticketNotificationService.SendNotificationAsync(updateTicketDataCommand.ToNotification());
    }

    public async Task UpdateDocumentation(UpdateTicketDocumentationCommand updateTicketDocumentationCommand)
    {
        await ticketEventStore.StoreEventAsync(eventTranslator.ToEvent(updateTicketDocumentationCommand));
        await ticketNotificationService.SendNotificationAsync(updateTicketDocumentationCommand.ToNotification());
    }

    public async Task Create(CreateTicketCommand createTicketCommand)
    {
        await ticketEventStore.StoreEventAsync(eventTranslator.ToEvent(createTicketCommand));
        await ticketNotificationService.SendNotificationAsync(createTicketCommand.ToNotification());
    }
}