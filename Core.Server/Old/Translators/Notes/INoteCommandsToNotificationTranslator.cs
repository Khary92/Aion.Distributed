using Service.Server.CQRS.Commands.Entities.Note;

namespace Service.Server.Old.Translators.Notes;

public interface INoteCommandsToNotificationTranslator
{
    NoteCreatedNotification ToNotification(CreateNoteCommand createNoteCommand);
    NoteUpdatedNotification ToNotification(UpdateNoteCommand updateNoteCommand);
}