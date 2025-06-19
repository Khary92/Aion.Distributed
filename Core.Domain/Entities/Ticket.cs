using System.Collections.ObjectModel;
using System.Text.Json;
using Domain.Events.Tickets;

namespace Domain.Entities;

public class Ticket
{
    public Guid TicketId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BookingNumber { get; set; } = string.Empty;
    public List<Guid> SprintIds { get; set; } = [];
    public string Documentation { get; set; } = string.Empty;

    public static Ticket Rehydrate(List<TicketEvent> events)
    {
        var ticket = new Ticket();
        foreach (var evt in events) ticket.Apply(evt);

        return ticket;
    }

    private void Apply(TicketEvent evt)
    {
        switch (evt.EventType)
        {
            case nameof(TicketCreatedEvent):
                var created = JsonSerializer.Deserialize<TicketCreatedEvent>(evt.EventPayload)!;
                TicketId = created.TicketId;
                Name = created.Name;
                BookingNumber = created.BookingNumber;
                Documentation = created.Documentation;
                SprintIds = created.SprintIds;
                break;

            case nameof(TicketDataUpdatedEvent):
                var dataUpdated = JsonSerializer.Deserialize<TicketDataUpdatedEvent>(evt.EventPayload)!;
                Name = dataUpdated.Name;
                BookingNumber = dataUpdated.BookingNumber;
                SprintIds = dataUpdated.SprintIds;
                break;

            case nameof(TicketDocumentationChangedEvent):
                var changed = JsonSerializer.Deserialize<TicketDocumentationChangedEvent>(evt.EventPayload)!;
                Documentation = changed.Documentation;
                break;
        }
    }
}