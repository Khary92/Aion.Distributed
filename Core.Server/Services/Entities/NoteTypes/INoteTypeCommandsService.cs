using Core.Server.Communication.Records.Commands.Entities.NoteType;

namespace Core.Server.Services.Entities.NoteTypes;

public interface INoteTypeCommandsService
{
    Task Create(CreateNoteTypeCommand command);
    Task ChangeName(ChangeNoteTypeNameCommand command);
    Task ChangeColor(ChangeNoteTypeColorCommand command);
}