using Domain.Events.Tags;
using Service.Server.CQRS.Commands.Entities.Tags;

namespace Service.Server.Old.Translators.Tags;

public interface ITagCommandsToEventTranslator
{
    TagEvent ToEvent(CreateTagCommand createTagCommand);
    TagEvent ToEvent(UpdateTagCommand updateTagCommand);
}