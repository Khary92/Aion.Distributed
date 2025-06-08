using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Avalonia.Communication.NotificationProcessors.Messages;
using Client.Avalonia.Communication.RequiresChange;
using Client.Avalonia.Factories;
using CommunityToolkit.Mvvm.Messaging;
using Contract.CQRS.Notifications.Entities.Note;
using Contract.DTO;
using MediatR;
using ReactiveUI;

namespace Client.Avalonia.ViewModels.TimeTracking.DynamicControls;

public class NoteStreamViewModel(
    IMediator mediator,
    IMessenger messenger,
    INoteViewFactory noteViewFactory,
    IRunTimeSettings runTimeSettings)
    : ReactiveObject
{
    private Guid _timeSlotId;
    public ObservableCollection<NoteViewModel> Notes { get; } = [];

    public Guid TimeSlotId
    {
        get => _timeSlotId;
        set => this.RaiseAndSetIfChanged(ref _timeSlotId, value);
    }

    public async Task AddNoteControl()
    {
        if (!runTimeSettings.IsSelectedDateCurrentDate()) return;

        var noteId = Guid.NewGuid();
        await mediator.Send(new CreateNoteCommand(noteId, string.Empty, Guid.Empty, TimeSlotId, DateTimeOffset.Now));
        await mediator.Send(new AddNoteCommand(TimeSlotId, noteId));
    }

    public void RegisterMessenger()
    {
        messenger.Register<NewNoteMessage>(this, async void (_, m) => { await InsertNoteViewModel(m.Note); });

        messenger.Register<NoteUpdatedNotification>(this, (_, m) =>
        {
            var viewModel = Notes.FirstOrDefault(n => n.Note.NoteId == m.NoteId);

            if (viewModel == null) return;

            viewModel.Note.Apply(m);
        });
    }

    public async Task InitializeAsync()
    {
        var noteDtos = await mediator.Send(new GetNotesByTimeSlotIdRequest(TimeSlotId));

        foreach (var note in noteDtos) await InsertNoteViewModel(note);
    }

    private async Task InsertNoteViewModel(NoteDto noteDto)
    {
        var noteViewModel = await noteViewFactory.Create(new NoteDto(noteDto.NoteId, noteDto.Text, noteDto.NoteTypeId,
            noteDto.TimeSlotId, noteDto.TimeStamp));

        Notes.Insert(0, noteViewModel);
    }
}