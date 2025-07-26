using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;

namespace Client.Desktop.Presentation.Factories;

public interface INoteViewFactory
{
    Task<NoteViewModel> Create(NoteClientModel noteClientModel);
}