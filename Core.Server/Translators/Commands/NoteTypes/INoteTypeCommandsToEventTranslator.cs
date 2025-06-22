using Core.Server.Communication.Records.Commands.Entities.NoteType;
using Domain.Events.NoteTypes;

namespace Core.Server.Translators.Commands.NoteTypes;

public interface INoteTypeCommandsToEventTranslator
{
    NoteTypeEvent ToEvent(CreateNoteTypeCommand createNoteTypeCommand);
    NoteTypeEvent ToEvent(ChangeNoteTypeNameCommand createNoteTypeCommand);
    NoteTypeEvent ToEvent(ChangeNoteTypeColorCommand createNoteTypeCommand);
}