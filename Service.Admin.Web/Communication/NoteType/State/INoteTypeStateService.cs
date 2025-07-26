using Service.Admin.Web.Communication.NoteType.Notifications;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.NoteType.State;

public interface INoteTypeStateService
{
    IReadOnlyList<NoteTypeWebModel> NoteTypes { get; }
    event Action? OnStateChanged;
    Task AddNoteType(NoteTypeWebModel noteType);
    void Apply(WebNoteTypeColorChangedNotification notification);
    void Apply(WebNoteTypeNameChangedNotification notification);
}