using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Proto.Requests.NoteTypes;

namespace Client.Desktop.Communication.Requests.NoteType;

public interface INoteTypeRequestSender
{
    Task<List<NoteTypeClientModel>> Send(GetAllNoteTypesRequestProto request);
    Task<NoteTypeClientModel> Send(GetNoteTypeByIdRequestProto request);
}