using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.NotificationWrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.RequiresChange;
using Client.Desktop.Factories;
using CommunityToolkit.Mvvm.Messaging;
using Contract.CQRS.Requests.Notes;
using Contract.DTO;
using Google.Protobuf.WellKnownTypes;
using MediatR;
using Proto.Command.Notes;
using Proto.Command.NoteTypes;
using Proto.Command.TimeSlots;
using Proto.Notifications.Note;
using Proto.Requests.Notes;
using ReactiveUI;

namespace Client.Desktop.Models.TimeTracking.DynamicControls;

public class NoteStreamViewModel(
    ICommandSender commandSender,
    IRequestSender requestSender,
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
        await commandSender.Send(new CreateNoteCommand
        {
            NoteId = noteId.ToString(),
            NoteTypeId = Guid.NewGuid().ToString(),
            Text = string.Empty,
            TimeSlotId = _timeSlotId.ToString(),
            TimeStamp = Timestamp.FromDateTimeOffset(DateTimeOffset.Now)
        });

        //TODO This must be done on server side. Obviously
        //await commandSender.Send(new AddNot
        //{
        //    NoteId = noteId.ToString(),
        //    TimeSlotId = TimeSlotId.ToString()
        //});
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
        var noteDtos = await requestSender.Send(new GetNotesByTimeSlotIdRequestProto
        {
            TimeSlotId = TimeSlotId.ToString()
        });

        foreach (var note in noteDtos) await InsertNoteViewModel(note);
    }

    private async Task InsertNoteViewModel(NoteDto noteDto)
    {
        var noteViewModel = await noteViewFactory.Create(new NoteDto(noteDto.NoteId, noteDto.Text, noteDto.NoteTypeId,
            noteDto.TimeSlotId, noteDto.TimeStamp));

        Notes.Insert(0, noteViewModel);
    }
}