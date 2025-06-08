using System;
using Client.Avalonia.ViewModels.TimeTracking.DynamicControls;

namespace Client.Avalonia.Factories;

public interface INoteStreamViewModelFactory
{
    NoteStreamViewModel Create(Guid timeSLotId);
}