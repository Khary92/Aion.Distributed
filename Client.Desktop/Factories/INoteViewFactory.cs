using System.Threading.Tasks;
using Client.Desktop.Models.TimeTracking.DynamicControls;
using Contract.DTO;

namespace Client.Desktop.Factories;

public interface INoteViewFactory
{
    Task<NoteViewModel> Create(NoteDto noteDto);
}