using Service.Admin.Web.Communication.NoteType.Notifications;
using Service.Admin.Web.DTO;

namespace Service.Admin.Web.Communication.NoteType.State;

public interface INoteTypeStateService
{
    IReadOnlyList<NoteTypeDto> NoteTypes { get; }
    event Action? OnStateChanged;
    Task AddNoteType(NoteTypeDto noteType);
    void Apply(WebNoteTypeColorChangedNotification notification);
    void Apply(WebNoteTypeNameChangedNotification notification);
    Task LoadNoteTypes();
}