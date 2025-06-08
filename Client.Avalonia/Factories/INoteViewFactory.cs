using System.Threading.Tasks;
using Client.Avalonia.ViewModels.TimeTracking.DynamicControls;
using Contract.DTO;

namespace Client.Avalonia.Factories;

public interface INoteViewFactory
{
    Task<NoteViewModel> Create(NoteDto noteDto);
}