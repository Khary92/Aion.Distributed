using Application.Contract.CQRS.Commands.Entities.Note;

namespace Application.Services.Entities.Notes;

public interface INoteCommandsService
{
    Task Update(UpdateNoteCommand updateNoteCommand);
    Task Create(CreateNoteCommand createNoteCommand);
}