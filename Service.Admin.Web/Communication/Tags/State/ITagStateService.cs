using Service.Admin.Web.Communication.Tags.Notifications;
using Service.Admin.Web.DTO;

namespace Service.Admin.Web.Communication.Tags.State;

public interface ITagStateService
{
    IReadOnlyList<TagDto> Tickets { get; }
    event Action? OnStateChanged;
    Task AddTicket(TagDto tag);
    void Apply(WebTagUpdatedNotification notification);
    Task LoadTags();
}