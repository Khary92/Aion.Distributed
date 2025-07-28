using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.Notes.Records;
using Client.Desktop.Communication.Notifications.Note.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Factories;
using Client.Desktop.Services.LocalSettings;
using CommunityToolkit.Mvvm.Messaging;
using Proto.Requests.Notes;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;

public class NoteStreamViewModel(
    ICommandSender commandSender,
    IRequestSender requestSender,
    ILocalSettingsService localSettingsService,
    IMessenger messenger,
    INoteViewFactory noteViewFactory)
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
        if (!localSettingsService.IsSelectedDateCurrentDate()) return;

        var noteId = Guid.NewGuid();
        await commandSender.Send(new ClientCreateNoteCommand(noteId, Guid.NewGuid(), string.Empty, TimeSlotId,
            DateTimeOffset.Now));
    }

    public void RegisterMessenger()
    {
        messenger.Register<NewNoteMessage>(this,
            async void (_, message) => { await InsertNoteViewModel(message.Note); });

        messenger.Register<ClientNoteUpdatedNotification>(this, (_, notification) =>
        {
            var viewModel = Notes.FirstOrDefault(n => n.Note.NoteId == notification.NoteId);

            if (viewModel == null) return;

            viewModel.Note.Apply(notification);
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

    private async Task InsertNoteViewModel(NoteClientModel noteClientModel)
    {
        var noteViewModel = await noteViewFactory.Create(new NoteClientModel(noteClientModel.NoteId,
            noteClientModel.Text, noteClientModel.NoteTypeId,
            noteClientModel.TimeSlotId, noteClientModel.TimeStamp));

        Notes.Insert(0, noteViewModel);
    }
}