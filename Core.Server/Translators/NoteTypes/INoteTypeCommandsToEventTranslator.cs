using Domain.Events.NoteTypes;
using Service.Server.CQRS.Commands.Entities.NoteType;

namespace Service.Server.Old.Translators.NoteTypes;

public interface INoteTypeCommandsToEventTranslator
{
    NoteTypeEvent ToEvent(CreateNoteTypeCommand createNoteTypeCommand);
    NoteTypeEvent ToEvent(ChangeNoteTypeNameCommand createNoteTypeCommand);
    NoteTypeEvent ToEvent(ChangeNoteTypeColorCommand createNoteTypeCommand);
}