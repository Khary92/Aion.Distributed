using Proto.DTO.NoteType;
using Proto.Requests.NoteTypes;

namespace Service.Proto.Shared.Requests.NoteTypes;

public interface INoteTypeRequestSender
{
    Task<GetAllNoteTypesResponseProto> Send(GetAllNoteTypesRequestProto request);
    Task<NoteTypeProto> Send(GetNoteTypeByIdRequestProto request);
}