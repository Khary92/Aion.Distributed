using System.Threading.Tasks;
using Client.Desktop.DTO;
using Client.Desktop.Models.TimeTracking.DynamicControls;

namespace Client.Desktop.Factories;

public interface INoteViewFactory
{
    Task<NoteViewModel> Create(NoteDto noteDto);
}