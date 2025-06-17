using Application.Contract.CQRS.Commands.Entities.Tags;
using Application.Contract.Notifications.Entities.Tags;

namespace Application.Translators.Tags;

public interface ITagCommandsToNotificationTranslator
{
    TagCreatedNotification ToNotification(CreateTagCommand createTagCommand);
    TagUpdatedNotification ToNotification(UpdateTagCommand updateTagCommand);
}