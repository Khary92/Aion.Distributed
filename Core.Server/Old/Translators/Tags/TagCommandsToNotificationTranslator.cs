using Service.Server.CQRS.Commands.Entities.Tags;

namespace Service.Server.Old.Translators.Tags;

public class TagCommandsToNotificationTranslator : ITagCommandsToNotificationTranslator
{
    public TagCreatedNotification ToNotification(CreateTagCommand createTagCommand)
    {
        return new TagCreatedNotification(createTagCommand.TagId, createTagCommand.Name);
    }

    public TagUpdatedNotification ToNotification(UpdateTagCommand updateTagCommand)
    {
        return new TagUpdatedNotification(updateTagCommand.TagId, updateTagCommand.Name);
    }
}