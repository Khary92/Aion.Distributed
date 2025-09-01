using Proto.Command.Sprints;
using Proto.Command.Tickets;
using Proto.DTO.TraceData;
using Proto.Notifications.Ticket;
using Service.Admin.Web.Communication.Tickets.Notifications;
using Service.Admin.Web.Communication.Tickets.Records;
using Service.Admin.Web.Communication.Wrappers;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.Tickets;

public static class TicketExtensions
{
    public static AddTicketToActiveSprintCommandProto ToProto(this WebAddTicketToSprintCommand command)
    {
        return new AddTicketToActiveSprintCommandProto
        {
            TicketId = command.TicketId.ToString(),
            TraceData = new TraceDataProto
            {
                TraceId = command.TraceId.ToString()
            }
        };
    }

    public static UpdateTicketDataCommandProto ToProto(this WebUpdateTicketCommand command)
    {
        return new UpdateTicketDataCommandProto
        {
            TicketId = command.TicketId.ToString(),
            Name = command.Name,
            BookingNumber = command.BookingNumber,
            SprintIds = { command.SprintIds.ToRepeatedField() },
            TraceData = new TraceDataProto
            {
                TraceId = command.TraceId.ToString()
            }
        };
    }

    public static CreateTicketCommandProto ToProto(this WebCreateTicketCommand command)
    {
        return new CreateTicketCommandProto
        {
            TicketId = command.TicketId.ToString(),
            BookingNumber = command.BookingNumber,
            Name = command.Name,
            SprintIds =
            {
                command.SprintIds.ToRepeatedField()
            },
            TraceData = new TraceDataProto
            {
                TraceId = command.TraceId.ToString()
            }
        };
    }

    public static NewTicketMessage ToNewEntityMessage(this TicketCreatedNotification notification)
    {
        return new NewTicketMessage(new TicketWebModel(
            Guid.Parse(notification.TicketId), notification.Name, notification.BookingNumber,
            notification.SprintIds.ToGuidList()), Guid.Parse(notification.TraceData.TraceId));
    }

    public static WebTicketDataUpdatedNotification ToNotification(this TicketDataUpdatedNotification notification)
    {
        return new WebTicketDataUpdatedNotification(Guid.Parse(notification.TicketId), notification.Name,
            notification.BookingNumber,
            notification.SprintIds.ToGuidList(), Guid.Parse(notification.TraceData.TraceId));
    }

    public static WebTicketDocumentationUpdatedNotification ToNotification(
        this TicketDocumentationUpdatedNotification notification)
    {
        return new WebTicketDocumentationUpdatedNotification(Guid.Parse(notification.TicketId),
            notification.Documentation,
            Guid.Parse(notification.TraceData.TraceId));
    }
}