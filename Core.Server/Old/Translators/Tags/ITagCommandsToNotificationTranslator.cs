using Service.Server.CQRS.Commands.Entities.Tags;

namespace Service.Server.Old.Translators.Tags;

public interface ITagCommandsToNotificationTranslator
{
    TagCreatedNotification ToNotification(CreateTagCommand createTagCommand);
    TagUpdatedNotification ToNotification(UpdateTagCommand updateTagCommand);
}