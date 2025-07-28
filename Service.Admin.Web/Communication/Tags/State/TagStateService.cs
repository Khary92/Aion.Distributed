using Proto.Requests.Tags;
using Service.Admin.Web.Communication.Tags.Notifications;
using Service.Admin.Web.Models;
using Service.Admin.Web.Services;

namespace Service.Admin.Web.Communication.Tags.State;

public class TagStateService(ISharedRequestSender requestSender) : ITagStateService, IInitializeAsync
{
    private List<TagWebModel> _tags = new();
    public IReadOnlyList<TagWebModel> Tickets => _tags.AsReadOnly();

    public event Action? OnStateChanged;

    public Task AddTicket(TagWebModel tag)
    {
        _tags.Add(tag);
        NotifyStateChanged();
        return Task.CompletedTask;
    }

    public void Apply(WebTagUpdatedNotification notification)
    {
        var tag = _tags.FirstOrDefault(x => x.TagId == notification.TagId);

        if (tag == null) return;

        tag.Apply(notification);
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
        _tags = tagListProto.ToDtoList();
        NotifyStateChanged();
    }
}