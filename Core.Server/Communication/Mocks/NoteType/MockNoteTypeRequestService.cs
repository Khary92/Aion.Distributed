using Grpc.Core;
using Proto.DTO.NoteType;
using Proto.Requests.NoteTypes;

namespace Core.Server.Communication.Mocks.NoteType;

public class MockNoteTypeRequestService : NoteTypesRequestService.NoteTypesRequestServiceBase
{
    public override Task<GetAllNoteTypesResponseProto> GetAllNoteTypes(GetAllNoteTypesRequestProto request,
        ServerCallContext context)
    {
        var response = new GetAllNoteTypesResponseProto();

        response.NoteTypes.Add(new NoteTypeProto
        {
            NoteTypeId = Guid.NewGuid().ToString(),
            Name = "Info",
            Color = "#00FF00"
        });
        response.NoteTypes.Add(new NoteTypeProto
        {
            NoteTypeId = Guid.NewGuid().ToString(),
            Name = "Warnung",
            Color = "#FFFF00"
        });

        return Task.FromResult(response);
    }

    public override Task<NoteTypeProto> GetNoteTypeById(GetNoteTypeByIdRequestProto request, ServerCallContext context)
    {
        var noteType = new NoteTypeProto
        {
            NoteTypeId = request.NoteTypeId,
            Name = request.NoteTypeId == "type-1" ? "Info" : "Unbekannt",
            Color = "#00FF00"
        };

        return Task.FromResult(noteType);
    }
}