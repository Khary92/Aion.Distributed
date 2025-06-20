using Grpc.Core;
using Proto.DTO.NoteType;
using Proto.Requests.NoteTypes;
using Service.Server.Old.Services.Entities.NoteTypes;

namespace Service.Server.Communication.NoteType;

public class NoteTypeRequestReceiver(INoteTypeRequestsService noteTypeRequestsService)
    : NoteTypesRequestService.NoteTypesRequestServiceBase
{
    public override async Task<GetAllNoteTypesResponseProto> GetAllNoteTypes(GetAllNoteTypesRequestProto request,
        ServerCallContext context)
    {
        var noteTypes = await noteTypeRequestsService.GetAll();
        return noteTypes.ToProtoList();
    }

    public override async Task<NoteTypeProto?> GetNoteTypeById(GetNoteTypeByIdRequestProto request,
        ServerCallContext context)
    {
        var noteType = await noteTypeRequestsService.GetById(Guid.Parse(request.NoteTypeId));
        return noteType?.ToProto();
    }
}