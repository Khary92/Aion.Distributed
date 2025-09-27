using Proto.Requests.NoteTypes;
using Service.Admin.Tracing;
using Service.Admin.Web.Communication.Mapper;
using Service.Admin.Web.Communication.Records.Notifications;
using Service.Admin.Web.Communication.Records.Wrappers;
using Service.Admin.Web.Communication.Sender.Common;
using Service.Admin.Web.Models;
using Service.Admin.Web.Services.Startup;

namespace Service.Admin.Web.Services.State;

public class NoteTypeStateService(ISharedRequestSender requestSender, ITraceCollector tracer)
    : INoteTypeStateService, IInitializeAsync
{
    private List<NoteTypeWebModel> _noteTypes = [];

    public InitializationType Type => InitializationType.StateService;

    public async Task InitializeComponents()
    {
        var noteTypeList = await requestSender.Send(new GetAllNoteTypesRequestProto());
        _noteTypes = noteTypeList.ToWebModelList();
    }

    public IReadOnlyList<NoteTypeWebModel> NoteTypes => _noteTypes.AsReadOnly();

    public event Action? OnStateChanged;

    public async Task AddNoteType(NewNoteTypeMessage noteTypeMessage)
    {
        _noteTypes.Add(noteTypeMessage.NoteType);
        await tracer.NoteType.Create.AggregateAdded(GetType(), noteTypeMessage.TraceId);
        NotifyStateChanged();
    }

    public async Task Apply(WebNoteTypeColorChangedNotification notification)
    {
        var noteType = _noteTypes.FirstOrDefault(nt => nt.NoteTypeId == notification.NoteTypeId);

        if (noteType == null)
        {
            await tracer.NoteType.ChangeColor.NoAggregateFound(GetType(), notification.TraceId);
            return;
        }

        noteType.Apply(notification);
        await tracer.NoteType.ChangeColor.ChangesApplied(GetType(), notification.TraceId);

        NotifyStateChanged();
    }

    public async Task Apply(WebNoteTypeNameChangedNotification notification)
    {
        var noteType = _noteTypes.FirstOrDefault(nt => nt.NoteTypeId == notification.NoteTypeId);

        if (noteType == null)
        {
            await tracer.NoteType.ChangeName.NoAggregateFound(GetType(), notification.TraceId);
            return;
        }

        noteType.Apply(notification);
        await tracer.NoteType.ChangeName.ChangesApplied(GetType(), notification.TraceId);
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        OnStateChanged?.Invoke();
    }
}