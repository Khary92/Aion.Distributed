using Domain.Events.Note;
using Service.Server.Communication.CQRS.Commands.Entities.Note;

namespace Service.Server.Translators.Notes;

public interface INoteCommandsToEventTranslator
{
    NoteEvent ToEvent(CreateNoteCommand createNoteCommand);
    NoteEvent ToEvent(UpdateNoteCommand updateNoteCommand);
}