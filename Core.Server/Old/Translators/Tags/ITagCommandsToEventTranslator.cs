using Application.Contract.CQRS.Commands.Entities.Tags;
using Domain.Events.Tags;

namespace Application.Translators.Tags;

public interface ITagCommandsToEventTranslator
{
    TagEvent ToEvent(CreateTagCommand createTagCommand);
    TagEvent ToEvent(UpdateTagCommand updateTagCommand);
}