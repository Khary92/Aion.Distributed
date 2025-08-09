using System;
using Client.Desktop.DataModels;
using Client.Proto;
using Proto.Notifications.Ticket;
using Proto.Requests.Tickets;

namespace Client.Desktop.Communication.Requests.Ticket;

public static class TicketRequestExtensions
{
    public static GetAllTicketsRequestProto ToProto(this ClientGetAllTicketsRequest request)
    {
        return new GetAllTicketsRequestProto();
    }

    public static GetTicketByIdRequestProto ToProto(this ClientGetTicketByIdRequest request)
    {
        return new GetTicketByIdRequestProto
        {
            TicketId = request.TicketId.ToString()
        };
    }

    public static GetTicketsForCurrentSprintRequestProto
        ToProto(this ClientGetTicketsForCurrentSprintRequest request)
    {
        return new GetTicketsForCurrentSprintRequestProto();
    }

    public static GetTicketsWithShowAllSwitchRequestProto
        ToProto(this ClientGetTicketsWithShowAllSwitchRequest request)
    {
        return new GetTicketsWithShowAllSwitchRequestProto
        {
            IsShowAll = request.IsShowAll
        };
    }

    public static TicketClientModel ToClientModel(this TicketCreatedNotification notification)
    {
        return new TicketClientModel(Guid.Parse(notification.TicketId), notification.Name, notification.BookingNumber,
            string.Empty, notification.SprintIds.ToGuidList());
    }
}