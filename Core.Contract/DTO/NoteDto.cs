using Contract.Notifications.Entities.Note;
using ReactiveUI;

namespace Contract.DTO;

public class NoteDto : ReactiveObject
{
    private Guid _noteId;
    private NoteTypeDto? _noteType;
    private Guid _noteTypeId;
    private string _text = string.Empty;
    private Guid _timeSlotId;
    private readonly DateTimeOffset _timeStamp;

    public NoteDto(Guid noteId, string text, Guid noteTypeId, Guid timeSlotId, DateTimeOffset timeStamp)
    {
        NoteId = noteId;
        Text = text;
        NoteTypeId = noteTypeId;
        TimeSlotId = timeSlotId;
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

    public NoteTypeDto? NoteType
    {
        get => _noteType;
        set => this.RaiseAndSetIfChanged(ref _noteType, value);
    }

    public void Apply(NoteUpdatedNotification notification)
    {
        NoteId = notification.NoteId;
        Text = notification.Text;
        NoteTypeId = notification.NoteTypeId;
        TimeSlotId = notification.TimeSlotId;
    }
}