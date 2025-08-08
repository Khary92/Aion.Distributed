using Proto.Requests.Tags;
using Service.Admin.Tracing;
using Service.Admin.Web.Communication.Tags.Notifications;
using Service.Admin.Web.Communication.Wrappers;
using Service.Admin.Web.Models;
using Service.Admin.Web.Services;

namespace Service.Admin.Web.Communication.Tags.State;

public class TagStateService(ISharedRequestSender requestSender, ITraceCollector tracer)
    : ITagStateService, IInitializeAsync
{
    private List<TagWebModel> _tags = new();
    public IReadOnlyList<TagWebModel> Tickets => _tags.AsReadOnly();

    public event Action? OnStateChanged;

    public async Task AddTag(NewTagMessage tagMessage)
    {
        _tags.Add(tagMessage.Tag);
        await tracer.Tag.Create.AggregateAdded(GetType(), tagMessage.TraceId);
        NotifyStateChanged();
    }

    public async Task Apply(WebTagUpdatedNotification notification)
    {
        var tag = _tags.FirstOrDefault(x => x.TagId == notification.TagId);

        if (tag == null)
        {
            await tracer.Tag.Update.NoAggregateFound(GetType(), notification.TraceId);
            return;
        }

        tag.Apply(notification);
        await tracer.Tag.Update.ChangesApplied(GetType(), notification.TraceId);
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        OnStateChanged?.Invoke();
    }

    public InitializationType Type => InitializationType.StateService;

    public async Task InitializeComponents()
    {
        var tagListProto = await requestSender.Send(new GetAllTagsRequestProto());
        _tags = tagListProto.ToWebModelList();
        NotifyStateChanged();
    }
}