using Proto.DTO.NoteType;
using Proto.Requests.NoteTypes;

namespace Service.Admin.Web.Communication.Requests.NoteTypes;

public interface INoteTypeRequestSender
{
    Task<GetAllNoteTypesResponseProto> Send(GetAllNoteTypesRequestProto request);
    Task<NoteTypeProto> Send(GetNoteTypeByIdRequestProto request);
}