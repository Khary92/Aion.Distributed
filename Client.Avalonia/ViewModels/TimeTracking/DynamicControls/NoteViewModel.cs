using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Avalonia.Communication.NotificationProcessors.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Contract.CQRS.Notifications.Entities.Note;
using Contract.CQRS.Notifications.Entities.NoteType;
using Contract.DTO;
using MediatR;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Avalonia.ViewModels.TimeTracking.DynamicControls;

public class NoteViewModel : ReactiveObject
{
    private readonly IMediator _mediator;
    private int _currentNoteTypeIndex;

    public NoteViewModel(IMessenger messenger, IMediator mediator)
    {
        _mediator = mediator;

        SetNextTypeCommand = ReactiveCommand.Create(SetNextType);
        SetPreviousTypeCommand = ReactiveCommand.Create(SetPreviousType);
        UpdateNoteCommand = ReactiveCommand.CreateFromTask(Update);

        messenger.Register<NoteUpdatedNotification>(this, (_, m) =>
        {
            if (Note.NoteId != m.NoteId) return;

            Note.Apply(m);
        });

        messenger.Register<NewNoteTypeMessage>(this, (_, m) => { NoteTypes.Add(m.NoteType); });

        messenger.Register<NoteTypeColorChangedNotification>(this, (_, m) =>
        {
            var noteType = NoteTypes.FirstOrDefault(nt => nt.NoteTypeId == m.NoteTypeId);

            if (noteType == null) return;

            noteType.Apply(m);
        });

        messenger.Register<NoteTypeNameChangedNotification>(this, (_, m) =>
        {
            var noteType = NoteTypes.FirstOrDefault(nt => nt.NoteTypeId == m.NoteTypeId);

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
        var noteTypeDtos = await _mediator.Send(new GetAllNoteTypesRequest());
        NoteTypes.AddRange(noteTypeDtos);

        if (Note.NoteTypeId == Guid.Empty) return;

        Note.NoteType = NoteTypes.First(nt => nt.NoteTypeId == Note.NoteTypeId);
    }

    private async Task Update()
    {
        await _mediator.Send(new UpdateNoteCommand(Note.NoteId, Note.Text, Note.NoteType!.NoteTypeId, Note.TimeSlotId));
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