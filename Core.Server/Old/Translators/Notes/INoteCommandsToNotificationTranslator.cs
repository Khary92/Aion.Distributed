using Application.Contract.CQRS.Commands.Entities.Note;
using Application.Contract.Notifications.Entities.Note;

namespace Application.Translators.Notes;

public interface INoteCommandsToNotificationTranslator
{
    NoteCreatedNotification ToNotification(CreateNoteCommand createNoteCommand);
    NoteUpdatedNotification ToNotification(UpdateNoteCommand updateNoteCommand);
}