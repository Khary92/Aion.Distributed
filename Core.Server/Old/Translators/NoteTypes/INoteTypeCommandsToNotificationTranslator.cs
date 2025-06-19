using Service.Server.CQRS.Commands.Entities.NoteType;

namespace Service.Server.Old.Translators.NoteTypes;

public interface INoteTypeCommandsToNotificationTranslator
{
    NoteTypeCreatedNotification ToNotification(CreateNoteTypeCommand createNoteTypeCommand);
    NoteTypeNameChangedNotification ToNotification(ChangeNoteTypeNameCommand createNoteTypeCommand);
    NoteTypeColorChangedNotification ToNotification(ChangeNoteTypeColorCommand createNoteTypeCommand);
}