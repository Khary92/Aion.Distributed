using System.Collections.ObjectModel;
using Contract.CQRS.Notifications.Entities.Tickets;
using Proto.Notification.Tickets;

namespace Contract.Proto.Converter.Notifications;

public static class TicketNotificationConverter
{
    public static TicketCreatedNotificationProto ToProto(TicketCreatedNotification notification)
        => new()
        {
            TicketId = notification.TicketId.ToString(),
            Name = notification.Name,
            BookingNumber = notification.BookingNumber,
            SprintIds = { notification.SprintIds.Select(guid => guid.ToString()) }
        };

    public static TicketCreatedNotification FromProto(TicketCreatedNotificationProto proto)
        => new(
            Guid.Parse(proto.TicketId),
            proto.Name,
            proto.BookingNumber,
            new Collection<Guid>(proto.SprintIds.Select(Guid.Parse).ToList())
        );

    public static TicketDataUpdatedNotificationProto ToProto(TicketDataUpdatedNotification notification)
        => new()
        {
            TicketId = notification.TicketId.ToString(),
            Name = notification.Name,
            BookingNumber = notification.BookingNumber,
            SprintIds = { notification.SprintIds.Select(guid => guid.ToString()) }
        };

    public static TicketDataUpdatedNotification FromProto(TicketDataUpdatedNotificationProto proto)
        => new(
            Guid.Parse(proto.TicketId),
            proto.Name,
            proto.BookingNumber,
            new Collection<Guid>(proto.SprintIds.Select(Guid.Parse).ToList())
        );

    public static TicketDocumentationUpdatedNotificationProto ToProto(TicketDocumentationUpdatedNotification notification)
        => new()
        {
            TicketId = notification.TicketId.ToString(),
            Documentation = notification.Documentation
        };

    public static TicketDocumentationUpdatedNotification FromProto(TicketDocumentationUpdatedNotificationProto proto)
        => new(
            Guid.Parse(proto.TicketId),
            proto.Documentation
        );
}