using Proto.Requests.NoteTypes;
using Service.Admin.Web.Communication.NoteType.Notifications;
using Service.Admin.Web.DTO;

namespace Service.Admin.Web.Communication.NoteType.State;

public class NoteTypeStateService(ISharedRequestSender requestSender) : INoteTypeStateService
{
    private List<NoteTypeDto> _noteTypes = new();
    public IReadOnlyList<NoteTypeDto> NoteTypes => _noteTypes.AsReadOnly();
    
    public event Action? OnStateChanged;
    public Task AddNoteType(NoteTypeDto noteType)
    {
        _noteTypes.Add(noteType);
        return Task.CompletedTask;
    }

    public void Apply(WebNoteTypeColorChangedNotification notification)
    {
        var noteType = _noteTypes.FirstOrDefault(nt => nt.NoteTypeId == notification.NoteTypeId);

        if (noteType == null)
        {
            return;
        }

        noteType.Apply(notification);
    }

    public void Apply(WebNoteTypeNameChangedNotification notification)
    {
        var noteType = _noteTypes.FirstOrDefault(nt => nt.NoteTypeId == notification.NoteTypeId);

        if (noteType == null)
        {
            return;
        }

        noteType.Apply(notification);
    }

    public async Task LoadNoteTypes()
    {
        var getAllNoteTypesResponseProto = await requestSender.Send(new GetAllNoteTypesRequestProto());
        _noteTypes = getAllNoteTypesResponseProto.ToDtoList();
    }
}