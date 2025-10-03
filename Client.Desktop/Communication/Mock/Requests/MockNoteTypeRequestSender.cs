using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Proto.DTO.NoteType;
using Proto.Requests.NoteTypes;
using Service.Proto.Shared.Requests.NoteTypes;

namespace Client.Desktop.Communication.Mock.Requests;

public class MockNoteTypeRequestSender(MockDataService mockDataService) : INoteTypeRequestSender
{
    public Task<GetAllNoteTypesResponseProto> Send(GetAllNoteTypesRequestProto request)
    {
        var protoData = mockDataService.NoteTypes.Select(ConvertToProto).ToList();

        var result = new GetAllNoteTypesResponseProto
        {
            NoteTypes = { protoData }
        };

        return Task.FromResult(result);
    }

    public Task<NoteTypeProto> Send(GetNoteTypeByIdRequestProto request)
    {
        return Task.FromResult(ConvertToProto(mockDataService.NoteTypes.First()));
    }

    private static NoteTypeProto ConvertToProto(NoteTypeClientModel noteTypeClientModel)
    {
        return new NoteTypeProto
        {
            NoteTypeId = noteTypeClientModel.NoteTypeId.ToString(),
            Name = noteTypeClientModel.Name,
            Color = noteTypeClientModel.Color
        };
    }
}