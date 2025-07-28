using System;
using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Desktop.Presentation.Factories;

public class NoteViewFactory(IServiceProvider serviceProvider) : INoteViewFactory
{
    public async Task<NoteViewModel> Create(NoteClientModel noteClientModel)
    {
        var noteViewModel = serviceProvider.GetRequiredService<NoteViewModel>();

        //TODO Clone
        noteViewModel.Note = new NoteClientModel(noteClientModel.NoteId, noteClientModel.Text,
            noteClientModel.NoteTypeId, noteClientModel.TimeSlotId,
            noteClientModel.TimeStamp);

        await noteViewModel.Initialize();
        noteViewModel.RegisterMessenger();

        return noteViewModel;
    }
}