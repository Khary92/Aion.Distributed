using System;
using System.Threading.Tasks;
using Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;

namespace Client.Desktop.Presentation.Factories;

public interface INoteStreamViewModelFactory
{
    Task<NoteStreamViewModel> Create(Guid timeSlotId, Guid ticketId);
}