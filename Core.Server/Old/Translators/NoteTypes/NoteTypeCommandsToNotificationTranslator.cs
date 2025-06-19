using Service.Server.CQRS.Commands.Entities.NoteType;

namespace Service.Server.Old.Translators.NoteTypes;

public class NoteTypeCommandsToNotificationTranslator : INoteTypeCommandsToNotificationTranslator
{
    public NoteTypeCreatedNotification ToNotification(CreateNoteTypeCommand createNoteTypeCommand)
    {
        return new NoteTypeCreatedNotification(createNoteTypeCommand.NoteTypeId, createNoteTypeCommand.Name,
            createNoteTypeCommand.Color);
    }

    public NoteTypeNameChangedNotification ToNotification(ChangeNoteTypeNameCommand createNoteTypeCommand)
    {
        return new NoteTypeNameChangedNotification(createNoteTypeCommand.NoteTypeId, createNoteTypeCommand.Name);
    }

    public NoteTypeColorChangedNotification ToNotification(ChangeNoteTypeColorCommand createNoteTypeCommand)
    {
        return new NoteTypeColorChangedNotification(createNoteTypeCommand.NoteTypeId, createNoteTypeCommand.Color);
    }
}