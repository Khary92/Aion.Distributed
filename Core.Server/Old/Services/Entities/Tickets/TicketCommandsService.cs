using Application.Contract.CQRS.Commands.Entities.Tickets;
using Application.Translators.Tickets;
using Domain.Interfaces;
using MediatR;

namespace Application.Services.Entities.Tickets;

public class TicketCommandsService(
    ITicketEventsStore ticketEventStore,
    IMediator mediator,
    ITicketCommandsToEventTranslator eventTranslator,
    ITicketCommandsToNotificationTranslator notificationTranslator)
    : ITicketCommandsService
{
    public async Task UpdateData(UpdateTicketDataCommand updateTicketDataCommand)
    {
        await ticketEventStore.StoreEventAsync(eventTranslator.ToEvent(updateTicketDataCommand));
        await mediator.Publish(notificationTranslator.ToNotification(updateTicketDataCommand));
    }

    public async Task UpdateDocumentation(UpdateTicketDocumentationCommand updateTicketDocumentationCommand)
    {
        await ticketEventStore.StoreEventAsync(eventTranslator.ToEvent(updateTicketDocumentationCommand));
        await mediator.Publish(notificationTranslator.ToNotification(updateTicketDocumentationCommand));
    }

    public async Task Create(CreateTicketCommand createTicketCommand)
    {
        await ticketEventStore.StoreEventAsync(eventTranslator.ToEvent(createTicketCommand));
        await mediator.Publish(notificationTranslator.ToNotification(createTicketCommand));
    }
}