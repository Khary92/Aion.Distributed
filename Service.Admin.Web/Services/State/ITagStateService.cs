using Service.Admin.Web.Communication.Records.Notifications;
using Service.Admin.Web.Communication.Records.Wrappers;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Services.State;

public interface ITagStateService
{
    IReadOnlyList<TagWebModel> Tickets { get; }
    event Action? OnStateChanged;
    Task AddTag(NewTagMessage tagMessage);
    Task Apply(WebTagUpdatedNotification notification);
}