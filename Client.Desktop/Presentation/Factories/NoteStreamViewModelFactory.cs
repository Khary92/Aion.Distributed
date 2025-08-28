using System;
using Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Desktop.Presentation.Factories;

public class NoteStreamViewModelFactory(IServiceProvider serviceProvider) : INoteStreamViewModelFactory
{
    public NoteStreamViewModel Create(Guid timeSlotId, Guid ticketId)
    {
        var notesStreamViewModel = serviceProvider.GetRequiredService<NoteStreamViewModel>();
        notesStreamViewModel.TicketId = ticketId;
        notesStreamViewModel.TimeSlotId = timeSlotId;

        return notesStreamViewModel;
    }
}