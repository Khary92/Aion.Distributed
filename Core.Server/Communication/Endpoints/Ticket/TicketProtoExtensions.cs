using Client.Proto;
using Core.Server.Communication.Records.Commands.Entities.Tickets;
using Proto.Command.Tickets;
using Proto.DTO.Ticket;
using Proto.Notifications.Ticket;
using Proto.Requests.Tickets;

namespace Core.Server.Communication.Endpoints.Ticket;

public static class TicketProtoExtensions
{
    public static UpdateTicketDataCommand ToCommand(this UpdateTicketDataCommandProto proto) => new(
        Guid.Parse(proto.TicketId), proto.Name, proto.BookingNumber,
        proto.SprintIds.ToGuidList(), Guid.Parse(proto.TraceData.TraceId));

    public static TicketNotification ToNotification(this UpdateTicketDataCommand proto) => new()
    {
        TicketDataUpdated = new TicketDataUpdatedNotification
        {
            TicketId = proto.TicketId.ToString(),
            Name = proto.Name,
            BookingNumber = proto.BookingNumber,
            SprintIds = { proto.SprintIds.ToRepeatedField() },
            TraceData = new()
            {
                TraceId = proto.TraceId.ToString()
            }
        }
    };

    public static UpdateTicketDocumentationCommand ToCommand(
        this UpdateTicketDocumentationCommandProto proto) => new(Guid.Parse(proto.TicketId), proto.Documentation,
        Guid.Parse(proto.TraceData.TraceId));

    public static TicketNotification ToNotification(this UpdateTicketDocumentationCommand proto) => new()
    {
        TicketDocumentationUpdated = new TicketDocumentationUpdatedNotification
        {
            TicketId = proto.TicketId.ToString(),
            Documentation = proto.Documentation
        }
    };

    public static CreateTicketCommand ToCommand(this CreateTicketCommandProto proto) => new(Guid.Parse(proto.TicketId),
        proto.Name, proto.BookingNumber, proto.SprintIds.ToGuidList(), Guid.Parse(proto.TraceData.TraceId));

    public static TicketNotification ToNotification(this CreateTicketCommand proto) => new()
    {
        TicketCreated = new TicketCreatedNotification
        {
            TicketId = proto.TicketId.ToString(),
            Name = proto.Name,
            BookingNumber = proto.BookingNumber,
            SprintIds = { proto.SprintIds.ToRepeatedField() },
            TraceData = new()
            {
                TraceId = proto.TraceId.ToString()
            }
        }
    };

    public static TicketProto ToProto(this Domain.Entities.Ticket ticket) => new()
    {
        TicketId = ticket.TicketId.ToString(),
        Name = ticket.Name,
        BookingNumber = ticket.BookingNumber,
        SprintIds = { ticket.SprintIds.ToRepeatedField() }
    };

    public static TicketListProto ToProtoList(this List<Domain.Entities.Ticket> tickets)
    {
        var ticketProtos = new TicketListProto();

        foreach (var ticket in tickets) ticketProtos.Tickets.Add(ticket.ToProto());

        return ticketProtos;
    }
}