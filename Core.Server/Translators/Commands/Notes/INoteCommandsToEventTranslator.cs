using Core.Server.Communication.Records.Commands.Entities.Note;
using Domain.Events.Note;

namespace Core.Server.Translators.Commands.Notes;

public interface INoteCommandsToEventTranslator
{
    NoteEvent ToEvent(CreateNoteCommand createNoteCommand);
    NoteEvent ToEvent(UpdateNoteCommand updateNoteCommand);
}