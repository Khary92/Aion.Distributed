using System;
using Client.Desktop.Communication.Notifications.Note.Records;
using ReactiveUI;

namespace Client.Desktop.DataModels;

public class NoteClientModel : ReactiveObject
{
    private readonly DateTimeOffset _timeStamp;
    private Guid _noteId;
    private NoteTypeClientModel? _noteType;
    private Guid _noteTypeId;
    private string _text = string.Empty;
    private Guid _ticketId;
    private Guid _timeSlotId;

    public NoteClientModel(Guid noteId, string text, Guid noteTypeId, Guid timeSlotId, Guid ticketId,
        DateTimeOffset timeStamp)
    {
        NoteId = noteId;
        Text = text;
        NoteTypeId = noteTypeId;
        TimeSlotId = timeSlotId;
        TicketId = ticketId;
        TimeStamp = timeStamp;
    }

    public DateTimeOffset TimeStamp
    {
        get => _timeStamp;
        private init => this.RaiseAndSetIfChanged(ref _timeStamp, value);
    }

    public Guid TimeSlotId
    {
        get => _timeSlotId;
        private set => this.RaiseAndSetIfChanged(ref _timeSlotId, value);
    }

    public Guid NoteId
    {
        get => _noteId;
        private set => this.RaiseAndSetIfChanged(ref _noteId, value);
    }

    public string Text
    {
        get => _text;
        set => this.RaiseAndSetIfChanged(ref _text, value);
    }

    public Guid NoteTypeId
    {
        get => _noteTypeId;
        private set => this.RaiseAndSetIfChanged(ref _noteTypeId, value);
    }

    public Guid TicketId
    {
        get => _ticketId;
        set => this.RaiseAndSetIfChanged(ref _ticketId, value);
    }

    public NoteTypeClientModel? NoteType
    {
        get => _noteType;
        set => this.RaiseAndSetIfChanged(ref _noteType, value);
    }

    public void Apply(ClientNoteUpdatedNotification notification)
    {
        NoteId = notification.NoteId;
        Text = notification.Text;
        NoteTypeId = notification.NoteTypeId;
        TimeSlotId = notification.TimeSlotId;
    }
}