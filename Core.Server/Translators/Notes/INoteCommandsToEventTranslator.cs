using Core.Server.Communication.CQRS.Commands.Entities.Note;
using Domain.Events.Note;

namespace Core.Server.Translators.Notes;

public interface INoteCommandsToEventTranslator
{
    NoteEvent ToEvent(CreateNoteCommand createNoteCommand);
    NoteEvent ToEvent(UpdateNoteCommand updateNoteCommand);
}