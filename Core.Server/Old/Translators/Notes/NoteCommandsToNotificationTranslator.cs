using Service.Server.CQRS.Commands.Entities.Note;

namespace Service.Server.Old.Translators.Notes;

public class NoteCommandsToNotificationTranslator : INoteCommandsToNotificationTranslator
{
    public NoteCreatedNotification ToNotification(CreateNoteCommand createNoteCommand)
    {
        return new NoteCreatedNotification(createNoteCommand.NoteId, createNoteCommand.Text,
            createNoteCommand.NoteTypeId, createNoteCommand.TimeSlotId, createNoteCommand.TimeStamp);
    }

    public NoteUpdatedNotification ToNotification(UpdateNoteCommand updateNoteCommand)
    {
        return new NoteUpdatedNotification(updateNoteCommand.NoteId, updateNoteCommand.Text,
            updateNoteCommand.NoteTypeId, updateNoteCommand.TimeSlotId);
    }
}