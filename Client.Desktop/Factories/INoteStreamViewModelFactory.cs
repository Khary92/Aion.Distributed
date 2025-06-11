using System;
using Client.Desktop.Models.TimeTracking.DynamicControls;

namespace Client.Desktop.Factories;

public interface INoteStreamViewModelFactory
{
    NoteStreamViewModel Create(Guid timeSLotId);
}