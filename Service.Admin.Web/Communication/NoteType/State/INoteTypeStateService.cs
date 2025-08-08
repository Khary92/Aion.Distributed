using Service.Admin.Web.Communication.NoteType.Notifications;
using Service.Admin.Web.Communication.Wrappers;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.NoteType.State;

public interface INoteTypeStateService
{
    IReadOnlyList<NoteTypeWebModel> NoteTypes { get; }
    event Action? OnStateChanged;
    Task AddNoteType(NewNoteTypeMessage noteTypeMessage);
    Task Apply(WebNoteTypeColorChangedNotification notification);
    Task Apply(WebNoteTypeNameChangedNotification notification);
}