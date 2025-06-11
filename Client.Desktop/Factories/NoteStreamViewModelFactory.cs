using System;
using Client.Desktop.Models.TimeTracking.DynamicControls;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Desktop.Factories;

public class NoteStreamViewModelFactory(IServiceProvider serviceProvider) : INoteStreamViewModelFactory
{
    public NoteStreamViewModel Create(Guid timeSlotId)
    {
        var notesStreamViewModel = serviceProvider.GetRequiredService<NoteStreamViewModel>();
        notesStreamViewModel.TimeSlotId = timeSlotId;

        return notesStreamViewModel;
    }
}