using System.Text.Json;
using Domain.Events.Note;

namespace Domain.Entities;

public class Note
{
    public Guid NoteId { get; set; }
    public string Text { get; set; } = string.Empty;
    public Guid NoteTypeId { get; set; }
    public Guid TicketId { get; set; }
    public Guid TimeSlotId { get; set; }
    public DateTimeOffset TimeStamp { get; set; }

    public static Note Rehydrate(IEnumerable<NoteEvent> events)
    {
        var documentationEntry = new Note();
        foreach (var evt in events) documentationEntry.Apply(evt);

        return documentationEntry;
    }

    private void Apply(NoteEvent evt)
    {
        switch (evt.EventType)
        {
            case nameof(NoteCreatedEvent):
                var created = JsonSerializer.Deserialize<NoteCreatedEvent>(evt.EventPayload);
                NoteId = created!.NoteId;
                Text = created.Text;
                NoteTypeId = created.NoteTypeId;
                TicketId = created.TicketId;
                TimeSlotId = created.TimeSlotId;
                TimeStamp = created.TimeStamp;
                break;

            case nameof(NoteUpdatedEvent):
                var updated = JsonSerializer.Deserialize<NoteUpdatedEvent>(evt.EventPayload);
                Text = updated!.Text;
                NoteTypeId = updated.NoteTypeId;
                break;
        }
    }
}