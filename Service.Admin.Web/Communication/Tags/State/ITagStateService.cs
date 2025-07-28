using Service.Admin.Web.Communication.Tags.Notifications;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.Tags.State;

public interface ITagStateService
{
    IReadOnlyList<TagWebModel> Tickets { get; }
    event Action? OnStateChanged;
    Task AddTicket(TagWebModel tag);
    void Apply(WebTagUpdatedNotification notification);
}