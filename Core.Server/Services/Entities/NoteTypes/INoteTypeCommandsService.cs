using Core.Server.Communication.Records.Commands.Entities.NoteType;

namespace Core.Server.Services.Entities.NoteTypes;

public interface INoteTypeCommandsService
{
    Task Create(CreateNoteTypeCommand createNoteTypeCommand);
    Task ChangeName(ChangeNoteTypeNameCommand changeNoteTypeNameCommand);
    Task ChangeColor(ChangeNoteTypeColorCommand changeNoteTypeColorCommand);
}