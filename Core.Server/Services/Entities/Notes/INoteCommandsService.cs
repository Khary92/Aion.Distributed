using Core.Server.Communication.CQRS.Commands.Entities.Note;

namespace Core.Server.Services.Entities.Notes;

public interface INoteCommandsService
{
    Task Update(UpdateNoteCommand updateNoteCommand);
    Task Create(CreateNoteCommand createNoteCommand);
}