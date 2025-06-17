using System.Text.Json;
using Domain.Events.NoteTypes;

namespace Domain.Entities;

public class NoteType
{
    public Guid NoteTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;

    public static NoteType Rehydrate(IEnumerable<NoteTypeEvent> events)
    {
        var noteType = new NoteType();
        foreach (var evt in events) noteType.Apply(evt);

        return noteType;
    }

    private void Apply(NoteTypeEvent evt)
    {
        switch (evt.EventType)
        {
            case nameof(NoteTypeCreatedEvent):
                var created = JsonSerializer.Deserialize<NoteTypeCreatedEvent>(evt.EventPayload);
                NoteTypeId = created!.NoteTypeId;
                Name = created.Name;
                Color = created.Color;
                break;

            case nameof(NoteTypeNameChangedEvent):
                var nameChanged = JsonSerializer.Deserialize<NoteTypeNameChangedEvent>(evt.EventPayload);
                Name = nameChanged!.Name;
                break;

            case nameof(NoteTypeColorChangedEvent):
                var colorChanged = JsonSerializer.Deserialize<NoteTypeColorChangedEvent>(evt.EventPayload);
                Color = colorChanged!.Color;
                break;
        }
    }
}