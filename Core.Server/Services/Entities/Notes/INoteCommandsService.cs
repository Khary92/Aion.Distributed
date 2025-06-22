using Core.Server.Communication.Records.Commands.Entities.Note;

namespace Core.Server.Services.Entities.Notes;

public interface INoteCommandsService
{
    Task Update(UpdateNoteCommand updateNoteCommand);
    Task Create(CreateNoteCommand createNoteCommand);
}