using System;
using Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;

namespace Client.Desktop.Presentation.Factories;

public interface INoteStreamViewModelFactory
{
    NoteStreamViewModel Create(Guid timeSlotId, Guid ticketId);
}