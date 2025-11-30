using Core.Server.Services.Entities.NoteTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Proto.DTO.NoteType;
using Proto.Requests.NoteTypes;

namespace Core.Server.Communication.Endpoints.NoteType;

[Authorize]
public class NoteTypeRequestReceiver(INoteTypeRequestsService noteTypeRequestsService)
    : NoteTypeProtoRequestService.NoteTypeProtoRequestServiceBase
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