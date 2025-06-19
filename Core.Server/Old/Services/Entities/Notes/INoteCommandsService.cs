using Service.Server.CQRS.Commands.Entities.Note;

namespace Service.Server.Old.Services.Entities.Notes;

public interface INoteCommandsService
{
    Task Update(UpdateNoteCommand updateNoteCommand);
    Task Create(CreateNoteCommand createNoteCommand);
}