using Core.Server.Communication.Records.Commands.Entities.Tags;
using Domain.Events.Tags;

namespace Core.Server.Translators.Commands.Tags;

public interface ITagCommandsToEventTranslator
{
    TagEvent ToEvent(CreateTagCommand createTagCommand);
    TagEvent ToEvent(UpdateTagCommand updateTagCommand);
}