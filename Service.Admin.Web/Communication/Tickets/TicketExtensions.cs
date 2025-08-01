using Proto.Command.Tickets;
using Proto.Notifications.Ticket;
using Service.Admin.Web.Communication.Tickets.Notifications;
using Service.Admin.Web.Communication.Tickets.Records;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.Tickets;

public static class TicketExtensions
{
    public static CreateTicketCommandProto ToProto(this WebCreateTicketCommand command) => new()
    {
        TicketId = command.TicketId.ToString(),
        BookingNumber = command.BookingNumber,
        Name = command.Name,
        SprintIds =
        {
            command.SprintIds.ToRepeatedField(),
        },
        TraceData = new()
        {
            TraceId = command.TraceId.ToString()
        }
    };

    public static TicketWebModel ToWebModel(this WebTicketCreatedNotification notification) => new(
        notification.TicketId, notification.Name, notification.BookingNumber,
        string.Empty, notification.SprintIds);

    public static WebTicketCreatedNotification ToNotification(this TicketCreatedNotification notification) => new(
        Guid.Parse(notification.TicketId), notification.Name, notification.BookingNumber,
        string.Empty, notification.SprintIds.ToGuidList(), Guid.Parse(notification.TraceData.TraceId));

    public static WebTicketDataUpdatedNotification ToNotification(this TicketDataUpdatedNotification notification) =>
        new(Guid.Parse(notification.TicketId), notification.Name,
            notification.BookingNumber, notification.SprintIds.ToGuidList());

    public static WebTicketDocumentationUpdatedNotification
        ToNotification(this TicketDocumentationUpdatedNotification notification) =>
        new(Guid.Parse(notification.TicketId), notification.Documentation);
}