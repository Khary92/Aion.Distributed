using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contract.DTO;

namespace Client.Desktop.Communication.Requests.NoteTypes;

public interface INoteTypesRequestSender
{
    Task<List<NoteTypeDto>> GetAllNoteTypes();
    Task<NoteTypeDto> GetNoteTypeById(Guid noteTypeId);
}