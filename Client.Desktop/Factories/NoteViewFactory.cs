using System;
using System.Threading.Tasks;
using Client.Desktop.DTO;
using Client.Desktop.Models.TimeTracking.DynamicControls;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Desktop.Factories;

public class NoteViewFactory(IServiceProvider serviceProvider) : INoteViewFactory
{
    public async Task<NoteViewModel> Create(NoteDto noteDto)
    {
        var noteViewModel = serviceProvider.GetRequiredService<NoteViewModel>();

        noteViewModel.Note = new NoteDto(noteDto.NoteId, noteDto.Text, noteDto.NoteTypeId, noteDto.TimeSlotId,
            noteDto.TimeStamp);

        await noteViewModel.Initialize();

        return noteViewModel;
    }
}