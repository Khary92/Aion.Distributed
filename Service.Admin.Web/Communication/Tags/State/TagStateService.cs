using Proto.Requests.Tags;
using Service.Admin.Web.Communication.Tags.Notifications;
using Service.Admin.Web.DTO;

namespace Service.Admin.Web.Communication.Tags.State;

public class TagStateService(ISharedRequestSender requestSender) : ITagStateService
{
    private List<TagDto> _tags = new();
    public IReadOnlyList<TagDto> Tickets => _tags.AsReadOnly();
    
    public event Action? OnStateChanged;
    
    public Task AddTicket(TagDto tag)
    {
        _tags.Add(tag);
        return Task.CompletedTask;
    }

    public void Apply(WebTagUpdatedNotification notification)
    {
        var tag = _tags.FirstOrDefault(x => x.TagId == notification.TagId);

        if (tag == null)
        {
            return;
        }
        
        tag.Apply(notification);
    }

    public async Task LoadTags()
    {
        var tagListProto = await requestSender.Send(new GetAllTagsRequestProto());
        _tags = tagListProto.ToDtoList();
        NotifyStateChanged();
    }
    
    private void NotifyStateChanged() => OnStateChanged?.Invoke();
}