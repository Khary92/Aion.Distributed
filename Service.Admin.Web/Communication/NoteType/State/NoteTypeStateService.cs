using Proto.Requests.NoteTypes;
using Service.Admin.Web.Communication.NoteType.Notifications;
using Service.Admin.Web.Models;
using Service.Admin.Web.Services;

namespace Service.Admin.Web.Communication.NoteType.State;

public class NoteTypeStateService(ISharedRequestSender requestSender) : INoteTypeStateService, IInitializeAsync
{
    private List<NoteTypeWebModel> _noteTypes = new();
    public IReadOnlyList<NoteTypeWebModel> NoteTypes => _noteTypes.AsReadOnly();

    public event Action? OnStateChanged;

    public Task AddNoteType(NoteTypeWebModel noteType)
    {
        _noteTypes.Add(noteType);
        return Task.CompletedTask;
    }

    public void Apply(WebNoteTypeColorChangedNotification notification)
    {
        var noteType = _noteTypes.FirstOrDefault(nt => nt.NoteTypeId == notification.NoteTypeId);

        if (noteType == null) return;

        noteType.Apply(notification);
        NotifyStateChanged();
    }

    public void Apply(WebNoteTypeNameChangedNotification notification)
    {
        var noteType = _noteTypes.FirstOrDefault(nt => nt.NoteTypeId == notification.NoteTypeId);

        if (noteType == null) return;

        noteType.Apply(notification);
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        OnStateChanged?.Invoke();
    }

    public InitializationType Type => InitializationType.StateService;
    public async Task InitializeComponents()
    {
        var noteTypeList = await requestSender.Send(new GetAllNoteTypesRequestProto());
        _noteTypes = noteTypeList.ToDtoList();
    }
}