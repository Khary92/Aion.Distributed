using System.Threading.Tasks;
using Grpc.Core;
using Proto.Requests.NoteTypes;

public class MockNoteTypeRequestService : NoteTypesRequestService.NoteTypesRequestServiceBase
{
    public override Task<GetAllNoteTypesResponseProto> GetAllNoteTypes(GetAllNoteTypesRequestProto request, ServerCallContext context)
    {
        var response = new GetAllNoteTypesResponseProto();

        // Beispielhafte Dummy-Daten - ersetze sie durch deine Logik
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
        // Dummy-Implementierung: je nach ID eine NoteType zurückgeben
        var noteType = new NoteTypeProto
        {
            NoteTypeId = request.NoteTypeId,
            Name = request.NoteTypeId == "type-1" ? "Info" : "Unbekannt",
            Color = "#00FF00"
        };

        return Task.FromResult(noteType);
    }
}