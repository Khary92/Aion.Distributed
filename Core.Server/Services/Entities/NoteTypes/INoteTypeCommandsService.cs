using Service.Server.Communication.CQRS.Commands.Entities.NoteType;

namespace Service.Server.Services.Entities.NoteTypes;

public interface INoteTypeCommandsService
{
    Task Create(CreateNoteTypeCommand createNoteTypeCommand);
    Task ChangeName(ChangeNoteTypeNameCommand changeNoteTypeNameCommand);
    Task ChangeColor(ChangeNoteTypeColorCommand changeNoteTypeColorCommand);
}