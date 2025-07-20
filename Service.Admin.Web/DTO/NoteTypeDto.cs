using Service.Admin.Web.Communication.NoteType.Notifications;

namespace Service.Admin.Web.DTO;

public class NoteTypeDto(Guid noteTypeId, string name, string color)
{
    public Guid NoteTypeId { get; private set; } = noteTypeId;
    public string Name { get; set; } = name;
    public string Color { get; set; } = color;

    public void Apply(WebNoteTypeNameChangedNotification notification)
    {
        Name = notification.Name;
    }

    public void Apply(WebNoteTypeColorChangedNotification notification)
    {
        Color = notification.Color;
    }
}