using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.Notes;
using Client.Desktop.Communication.RequiresChange;
using Client.Desktop.Factories;
using CommunityToolkit.Mvvm.Messaging;
using Contract.CQRS.Requests.Notes;
using Contract.DTO;
using Google.Protobuf.WellKnownTypes;
using MediatR;
using Proto.Command.Notes;
using Proto.Command.TimeSlots;
using Proto.Notifications.Note;
using ReactiveUI;

namespace Client.Desktop.Models.TimeTracking.DynamicControls;

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
        await mediator.Send(new CreateNoteCommand
        {
            NoteId = noteId.ToString(),
            Text = string.Empty,
            NoteTypeId = Guid.Empty.ToString(),
            TimeSlotId = TimeSlotId.ToString(),
            TimeStamp = Timestamp.FromDateTimeOffset(DateTimeOffset.Now)
        });

        await mediator.Send(new AddNoteCommand
        {
            NoteId = noteId.ToString(),
            TimeSlotId = TimeSlotId.ToString()
        });
    }

    public void RegisterMessenger()
    {
        messenger.Register<NewNoteMessage>(this, async void (_, m) => { await InsertNoteViewModel(m.Note); });

        messenger.Register<NoteUpdatedNotification>(this, (_, m) =>
        {
            var viewModel = Notes.FirstOrDefault(n => n.Note.NoteId == Guid.Parse(m.NoteId));

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