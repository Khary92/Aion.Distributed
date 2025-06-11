using System;
using Client.Avalonia.Models.TimeTracking.DynamicControls;

namespace Client.Avalonia.Factories;

public interface INoteStreamViewModelFactory
{
    NoteStreamViewModel Create(Guid timeSLotId);
}