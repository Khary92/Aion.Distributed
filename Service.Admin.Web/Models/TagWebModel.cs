using Service.Admin.Web.Communication.Records.Notifications;

namespace Service.Admin.Web.Models;

public class TagWebModel(Guid ticketId, string name)
{
    public Guid TagId { get; private set; } = ticketId;
    public string Name { get; set; } = name;

    public void Apply(WebTagUpdatedNotification notification)
    {
        Name = notification.Name;
    }
}