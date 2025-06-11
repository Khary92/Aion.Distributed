using System.Collections.Generic;
using System.Threading.Tasks;
using Contract.DTO;
using Proto.Requests.NoteTypes;

namespace Client.Avalonia.Communication.Requests.NoteTypes;

public interface INoteTypesRequestSender
{
    Task<List<NoteTypeDto>> GetAllNoteTypes();
    Task<NoteTypeDto> GetNoteTypeById(string noteTypeId);
}