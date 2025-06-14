using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contract.DTO;
using Proto.Requests.NoteTypes;

namespace Client.Desktop.Communication.Requests.NoteTypes;

public interface INoteTypesRequestSender
{
    Task<List<NoteTypeDto>> Send(GetAllNoteTypesRequestProto request);
    Task<NoteTypeDto> Send(GetNoteTypeByIdRequestProto request);
}