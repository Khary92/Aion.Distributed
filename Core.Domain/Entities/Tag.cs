using System.Text.Json;
using Domain.Events.Tags;

namespace Domain.Entities;

public class Tag
{
    public Guid TagId { get; set; }
    public string Name { get; set; } = string.Empty;

    public static Tag Rehydrate(IEnumerable<TagEvent> events)
    {
        var tag = new Tag();
        foreach (var evt in events) tag.Apply(evt);
        return tag;
    }

    private void Apply(TagEvent evt)
    {
        switch (evt.EventType)
        {
            case nameof(TagCreatedEvent):
                var created = JsonSerializer.Deserialize<TagCreatedEvent>(evt.EventPayload);
                TagId = created!.TagId;
                Name = created.Name;
                break;

            case nameof(TagUpdatedEvent):
                var updated = JsonSerializer.Deserialize<TagUpdatedEvent>(evt.EventPayload);
                Name = updated!.Name;
                break;
        }
    }
}