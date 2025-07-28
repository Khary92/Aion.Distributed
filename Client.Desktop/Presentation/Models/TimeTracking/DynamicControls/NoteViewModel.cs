using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.Notes.Records;
using Client.Desktop.Communication.Notifications.Note.Records;
using Client.Desktop.Communication.Notifications.NoteType.Records;
using Client.Desktop.Communication.Notifications.Wrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.Requests.NoteType;
using Client.Desktop.DataModels;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using Proto.Requests.NoteTypes;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;

public class NoteViewModel : ReactiveObject
{
    private readonly ICommandSender _commandSender;
    private readonly IMessenger _messenger;
    private readonly IRequestSender _requestSender;
    private int _currentNoteTypeIndex;

    public NoteViewModel(ICommandSender commandSender, IRequestSender requestSender, IMessenger messenger)
    {
        _commandSender = commandSender;
        _requestSender = requestSender;
        _messenger = messenger;

        SetNextTypeCommand = ReactiveCommand.Create(SetNextType);
        SetPreviousTypeCommand = ReactiveCommand.Create(SetPreviousType);
        UpdateNoteCommand = ReactiveCommand.CreateFromTask(Update);
    }

    public NoteClientModel Note { get; set; } = null!;

    private ObservableCollection<NoteTypeClientModel> NoteTypes { get; } = [];
    public ReactiveCommand<Unit, Unit> SetNextTypeCommand { get; }
    public ReactiveCommand<Unit, Unit> SetPreviousTypeCommand { get; }
    public ReactiveCommand<Unit, Unit> UpdateNoteCommand { get; }

    public void RegisterMessenger()
    {
        _messenger.Register<ClientNoteUpdatedNotification>(this, (_, notification) =>
        {
            if (Note.NoteId != notification.NoteId) return;

            Note.Apply(notification);
        });

        _messenger.Register<NewNoteTypeMessage>(this, (_, message) => { NoteTypes.Add(message.NoteType); });

        _messenger.Register<ClientNoteTypeColorChangedNotification>(this, (_, notification) =>
        {
            var noteType = NoteTypes.FirstOrDefault(nt => nt.NoteTypeId == notification.NoteTypeId);

            if (noteType == null) return;

            noteType.Apply(notification);
        });

        _messenger.Register<ClientNoteTypeNameChangedNotification>(this, (_, notification) =>
        {
            var noteType = NoteTypes.FirstOrDefault(nt => nt.NoteTypeId == notification.NoteTypeId);

            if (noteType == null) return;

            noteType.Apply(notification);
        });
    }

    public async Task Initialize()
    {
        NoteTypes.Clear();
        var noteTypeDtos = await _requestSender.Send(new ClientGetAllNoteTypesRequest());
        NoteTypes.AddRange(noteTypeDtos);

        if (Note.NoteTypeId == Guid.Empty || !NoteTypes.Any()) return;

        Note.NoteType = NoteTypes.FirstOrDefault(nt => nt.NoteTypeId == Note.NoteTypeId);
    }

    private async Task Update()
    {
        await _commandSender.Send(new ClientUpdateNoteCommand
        (
            Note.NoteId,
            Note.Text,
            Note.NoteType?.NoteTypeId ?? Guid.Empty, Note.TimeSlotId
        ));
    }

    private void SetNextType()
    {
        if (NoteTypes.Count == 0) return;

        if (_currentNoteTypeIndex >= NoteTypes.Count - 1) return;

        _currentNoteTypeIndex++;
        Note.NoteType = NoteTypes[_currentNoteTypeIndex];
    }

    private void SetPreviousType()
    {
        if (NoteTypes.Count == 0 || _currentNoteTypeIndex == 0) return;

        _currentNoteTypeIndex--;
        Note.NoteType = NoteTypes[_currentNoteTypeIndex];
    }
}