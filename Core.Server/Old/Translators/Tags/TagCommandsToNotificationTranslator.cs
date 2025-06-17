using Application.Contract.CQRS.Commands.Entities.Tags;
using Application.Contract.Notifications.Entities.Tags;

namespace Application.Translators.Tags;

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