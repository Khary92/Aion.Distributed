using System.Text.Json;
using Core.Server.Communication.Records.Commands.Entities.Tickets;
using Domain.Events.Tickets;

namespace Core.Server.Translators.Commands.Tickets;

public class TicketCommandsToEventTranslator : ITicketCommandsToEventTranslator
{
    public TicketEvent ToEvent(CreateTicketCommand createTicketCommand)
    {
        var domainEvent = new TicketCreatedEvent(createTicketCommand.TicketId, createTicketCommand.Name,
            createTicketCommand.BookingNumber, string.Empty, createTicketCommand.SprintIds);

        return CreateDatabaseEvent(nameof(TicketCreatedEvent), createTicketCommand.TicketId,
            JsonSerializer.Serialize(domainEvent));
    }

    public TicketEvent ToEvent(UpdateTicketDataCommand updateTicketDataCommand)
    {
        var domainEvent = new TicketDataUpdatedEvent(updateTicketDataCommand.TicketId, updateTicketDataCommand.Name,
            updateTicketDataCommand.BookingNumber, updateTicketDataCommand.SprintIds);

        return CreateDatabaseEvent(nameof(TicketDataUpdatedEvent), updateTicketDataCommand.TicketId,
            JsonSerializer.Serialize(domainEvent));
    }

    public TicketEvent ToEvent(UpdateTicketDocumentationCommand updateTicketDocumentationCommand)
    {
        var domainEvent = new TicketDocumentationChangedEvent(updateTicketDocumentationCommand.TicketId,
            updateTicketDocumentationCommand.Documentation);

        return CreateDatabaseEvent(nameof(TicketDocumentationChangedEvent), updateTicketDocumentationCommand.TicketId,
            JsonSerializer.Serialize(domainEvent));
    }

    private static TicketEvent CreateDatabaseEvent(string eventName, Guid entityId, string json)
    {
        return new TicketEvent(Guid.NewGuid(), DateTimeOffset.Now, eventName, entityId, json);
    }
}