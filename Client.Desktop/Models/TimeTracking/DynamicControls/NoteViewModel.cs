using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.NotificationWrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.DTO;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using Proto.Command.Notes;
using Proto.Notifications.Note;
using Proto.Notifications.NoteType;
using Proto.Requests.NoteTypes;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Models.TimeTracking.DynamicControls;

public class NoteViewModel : ReactiveObject
{
    private readonly ICommandSender _commandSender;
    private readonly IRequestSender _requestSender;
    private int _currentNoteTypeIndex;

    public NoteViewModel(ICommandSender commandSender, IRequestSender requestSender, IMessenger messenger)
    {
        _commandSender = commandSender;
        _requestSender = requestSender;

        SetNextTypeCommand = ReactiveCommand.Create(SetNextType);
        SetPreviousTypeCommand = ReactiveCommand.Create(SetPreviousType);
        UpdateNoteCommand = ReactiveCommand.CreateFromTask(Update);

        messenger.Register<NoteUpdatedNotification>(this, (_, m) =>
        {
            if (Note.NoteId != Guid.Parse(m.NoteId)) return;

            Note.Apply(m);
        });

        messenger.Register<NewNoteTypeMessage>(this, (_, m) => { NoteTypes.Add(m.NoteType); });

        messenger.Register<NoteTypeColorChangedNotification>(this, (_, m) =>
        {
            var noteType = NoteTypes.FirstOrDefault(nt => nt.NoteTypeId == Guid.Parse(m.NoteTypeId));

            if (noteType == null) return;

            noteType.Apply(m);
        });

        messenger.Register<NoteTypeNameChangedNotification>(this, (_, m) =>
        {
            var noteType = NoteTypes.FirstOrDefault(nt => nt.NoteTypeId == Guid.Parse(m.NoteTypeId));

            if (noteType == null) return;

            noteType.Apply(m);
        });
    }

    public NoteDto Note { get; set; } = null!;

    private ObservableCollection<NoteTypeDto> NoteTypes { get; } = [];
    public ReactiveCommand<Unit, Unit> SetNextTypeCommand { get; }
    public ReactiveCommand<Unit, Unit> SetPreviousTypeCommand { get; }
    public ReactiveCommand<Unit, Unit> UpdateNoteCommand { get; }

    public async Task Initialize()
    {
        NoteTypes.Clear();
        var noteTypeDtos = await _requestSender.Send(new GetAllNoteTypesRequestProto());
        NoteTypes.AddRange(noteTypeDtos);

        if (Note.NoteTypeId == Guid.Empty || !NoteTypes.Any()) return;

        Note.NoteType = NoteTypes.FirstOrDefault(nt => nt.NoteTypeId == Note.NoteTypeId);
    }

    private async Task Update()
    {
        await _commandSender.Send(new UpdateNoteCommandProto
        {
            NoteId = Note.NoteId.ToString(),
            Text = Note.Text,
            NoteTypeId = Note.NoteTypeId.ToString(),
            TimeSlotId = Note.TimeSlotId.ToString()
        });
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