using Application.Contract.CQRS.Commands.Entities.NoteType;
using Domain.Events.NoteTypes;

namespace Application.Translators.NoteTypes;

public interface INoteTypeCommandsToEventTranslator
{
    NoteTypeEvent ToEvent(CreateNoteTypeCommand createNoteTypeCommand);
    NoteTypeEvent ToEvent(ChangeNoteTypeNameCommand createNoteTypeCommand);
    NoteTypeEvent ToEvent(ChangeNoteTypeColorCommand createNoteTypeCommand);
}