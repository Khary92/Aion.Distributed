using System.Threading.Tasks;
using Proto.Requests.NoteTypes;

namespace Client.Avalonia.Communication.Requests;

public interface INoteTypesRequestSender
{
    Task<GetAllNoteTypesResponseProto> GetAllNoteTypes();
    Task<NoteTypeProto> GetNoteTypeById(string noteTypeId);
}