using Core.Server.Communication.CQRS.Commands.Entities.NoteType;
using Domain.Events.NoteTypes;

namespace Core.Server.Translators.NoteTypes;

public interface INoteTypeCommandsToEventTranslator
{
    NoteTypeEvent ToEvent(CreateNoteTypeCommand createNoteTypeCommand);
    NoteTypeEvent ToEvent(ChangeNoteTypeNameCommand createNoteTypeCommand);
    NoteTypeEvent ToEvent(ChangeNoteTypeColorCommand createNoteTypeCommand);
}