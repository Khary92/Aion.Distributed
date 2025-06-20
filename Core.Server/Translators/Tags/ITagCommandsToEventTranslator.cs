using Core.Server.Communication.CQRS.Commands.Entities.Tags;
using Domain.Events.Tags;

namespace Core.Server.Translators.Tags;

public interface ITagCommandsToEventTranslator
{
    TagEvent ToEvent(CreateTagCommand createTagCommand);
    TagEvent ToEvent(UpdateTagCommand updateTagCommand);
}