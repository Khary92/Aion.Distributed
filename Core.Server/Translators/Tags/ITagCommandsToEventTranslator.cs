using Domain.Events.Tags;
using Service.Server.Communication.CQRS.Commands.Entities.Tags;

namespace Service.Server.Translators.Tags;

public interface ITagCommandsToEventTranslator
{
    TagEvent ToEvent(CreateTagCommand createTagCommand);
    TagEvent ToEvent(UpdateTagCommand updateTagCommand);
}