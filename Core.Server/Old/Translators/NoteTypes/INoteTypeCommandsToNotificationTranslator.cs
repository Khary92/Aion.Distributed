using Application.Contract.CQRS.Commands.Entities.NoteType;
using Application.Contract.Notifications.Entities.NoteType;

namespace Application.Translators.NoteTypes;

public interface INoteTypeCommandsToNotificationTranslator
{
    NoteTypeCreatedNotification ToNotification(CreateNoteTypeCommand createNoteTypeCommand);
    NoteTypeNameChangedNotification ToNotification(ChangeNoteTypeNameCommand createNoteTypeCommand);
    NoteTypeColorChangedNotification ToNotification(ChangeNoteTypeColorCommand createNoteTypeCommand);
}