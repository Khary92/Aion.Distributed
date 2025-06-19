using Domain.Events.Note;
using Service.Server.CQRS.Commands.Entities.Note;

namespace Service.Server.Old.Translators.Notes;

public interface INoteCommandsToEventTranslator
{
    NoteEvent ToEvent(CreateNoteCommand createNoteCommand);
    NoteEvent ToEvent(UpdateNoteCommand updateNoteCommand);
}