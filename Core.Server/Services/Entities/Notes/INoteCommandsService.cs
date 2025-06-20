using Service.Server.Communication.CQRS.Commands.Entities.Note;

namespace Service.Server.Services.Entities.Notes;

public interface INoteCommandsService
{
    Task Update(UpdateNoteCommand updateNoteCommand);
    Task Create(CreateNoteCommand createNoteCommand);
}