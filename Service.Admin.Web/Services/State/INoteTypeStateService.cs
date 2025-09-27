using Service.Admin.Web.Communication.Records.Notifications;
using Service.Admin.Web.Communication.Records.Wrappers;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Services.State;

public interface INoteTypeStateService
{
    IReadOnlyList<NoteTypeWebModel> NoteTypes { get; }
    event Action? OnStateChanged;
    Task AddNoteType(NewNoteTypeMessage noteTypeMessage);
    Task Apply(WebNoteTypeColorChangedNotification notification);
    Task Apply(WebNoteTypeNameChangedNotification notification);
}