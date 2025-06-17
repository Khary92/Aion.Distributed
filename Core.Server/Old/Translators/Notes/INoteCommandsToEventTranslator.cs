using Application.Contract.CQRS.Commands.Entities.Note;
using Domain.Events.Note;

namespace Application.Translators.Notes;

public interface INoteCommandsToEventTranslator
{
    NoteEvent ToEvent(CreateNoteCommand createNoteCommand);
    NoteEvent ToEvent(UpdateNoteCommand updateNoteCommand);
}