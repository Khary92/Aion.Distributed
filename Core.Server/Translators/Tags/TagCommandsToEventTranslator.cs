using System.Text.Json;
using Domain.Events.Tags;
using Service.Server.Communication.CQRS.Commands.Entities.Tags;

namespace Service.Server.Translators.Tags;

public class TagCommandsToEventTranslator : ITagCommandsToEventTranslator
{
    public TagEvent ToEvent(CreateTagCommand createTagCommand)
    {
        var domainEvent = new TagCreatedEvent(createTagCommand.TagId, createTagCommand.Name);

        return CreateDatabaseEvent(nameof(TagCreatedEvent), createTagCommand.TagId,
            JsonSerializer.Serialize(domainEvent));
    }

    public TagEvent ToEvent(UpdateTagCommand updateTagCommand)
    {
        var domainEvent = new TagUpdatedEvent(updateTagCommand.TagId, updateTagCommand.Name);

        return CreateDatabaseEvent(nameof(TagUpdatedEvent), updateTagCommand.TagId,
            JsonSerializer.Serialize(domainEvent));
    }

    private static TagEvent CreateDatabaseEvent(string eventName, Guid entityId, string json)
    {
        return new TagEvent(Guid.NewGuid(), DateTime.UtcNow, TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow),
            eventName,
            entityId, json);
    }
}