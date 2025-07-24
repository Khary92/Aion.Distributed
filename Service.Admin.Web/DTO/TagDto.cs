using Service.Admin.Web.Communication.Tags.Notifications;

namespace Service.Admin.Web.DTO;

public class TagDto(Guid ticketId, string name, bool isSelected)
{
    public Guid TagId { get; private set; } = ticketId;
    public string Name { get; set; } = name;
    public bool IsDeleted { get; set; } = isSelected;

    public void Apply(WebTagUpdatedNotification notification)
    {
        Name = notification.Name;
    }
}