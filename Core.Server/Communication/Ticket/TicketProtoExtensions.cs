using Proto.Command.Tickets;
using Proto.DTO.Ticket;
using Proto.Notifications.Ticket;
using Proto.Requests.Tickets;
using Service.Server.CQRS.Commands.Entities.Tickets;

namespace Service.Server.Communication.Ticket;

public static class TicketProtoExtensions
{
    public static UpdateTicketDataCommand ToCommand(
        this UpdateTicketDataCommandProto proto) =>
        new(Guid.Parse(proto.TicketId), proto.Name, proto.BookingNumber, proto.SprintIds.ToGuidList());

    public static TicketNotification ToNotification(this UpdateTicketDataCommand proto) =>
        new()
        {
            TicketDataUpdated = new TicketDataUpdatedNotification()
            {
                TicketId = proto.TicketId.ToString(),
                Name = proto.Name,
                BookingNumber = proto.BookingNumber,
                SprintIds = { proto.SprintIds.ToRepeatedField() }
            }
        };

    public static UpdateTicketDocumentationCommand ToCommand(
        this UpdateTicketDocumentationCommandProto proto) =>
        new(Guid.Parse(proto.TicketId), proto.Documentation);

    public static TicketNotification ToNotification(this UpdateTicketDocumentationCommand proto) =>
        new()
        {
            TicketDocumentationUpdated = new TicketDocumentationUpdatedNotification
            {
                TicketId = proto.TicketId.ToString(),
                Documentation = proto.Documentation
            }
        };

    public static CreateTicketCommand ToCommand(
        this CreateTicketCommandProto proto) =>
        new(Guid.Parse(proto.TicketId), proto.Name, proto.BookingNumber, proto.SprintIds.ToGuidList());

    public static TicketNotification ToNotification(this CreateTicketCommand proto) =>
        new()
        {
            TicketCreated = new TicketCreatedNotification()
            {
                TicketId = proto.TicketId.ToString(),
                Name = proto.Name,
                BookingNumber = proto.BookingNumber,
                SprintIds = { proto.SprintIds.ToRepeatedField() }
            }
        };

    public static TicketProto ToProto(this Domain.Entities.Ticket ticket) =>
        new()
        {
            TicketId = ticket.TicketId.ToString(),
            Name = ticket.Name,
            BookingNumber = ticket.BookingNumber,
            SprintIds = { ticket.SprintIds.ToRepeatedField() }
        };

    public static TicketListProto ToProtoList(this List<Domain.Entities.Ticket> tickets)
    {
        var ticketProtos = new TicketListProto();

        foreach (var ticket in tickets)
        {
            ticketProtos.Tickets.Add(ticket.ToProto());
        }

        return ticketProtos;
    }
}