using Application.Contract.CQRS.Commands.Entities.NoteType;

namespace Application.Services.Entities.NoteTypes;

public interface INoteTypeCommandsService
{
    Task Create(CreateNoteTypeCommand createNoteTypeCommand);
    Task ChangeName(ChangeNoteTypeNameCommand changeNoteTypeNameCommand);
    Task ChangeColor(ChangeNoteTypeColorCommand changeNoteTypeColorCommand);
}