using Google.Protobuf.Collections;
using Proto.DTO.Sprint;
using Proto.DTO.Ticket;
using Proto.Requests.Tickets;
using Service.Admin.Web.DTO;

namespace Service.Admin.Web.Communication;

public static class DtoExtensions
{
    public static List<TicketDto> ToDtoList(this TicketListProto? ticketListProto)
    {
        if (ticketListProto == null) return [];

        return ticketListProto.Tickets
            .Select(ToDto)
            .ToList();
    }

    public static TicketDto ToDto(this TicketProto ticket)
    {
        var sprintIds = ticket.SprintIds
            .Select(idStr => Guid.TryParse(idStr, out var guid) ? guid : Guid.Empty)
            .Where(guid => guid != Guid.Empty)
            .ToList();

        return new TicketDto(
            Guid.Parse(ticket.TicketId),
            ticket.Name,
            ticket.BookingNumber,
            ticket.Documentation,
            [..sprintIds]
        );
    }

    public static List<Guid> ToGuidList(this RepeatedField<string> stringGuids)
    {
        var guids = new List<Guid>();
        foreach (var id in stringGuids)
            if (Guid.TryParse(id, out var guid))
                guids.Add(guid);

        return guids;
    }

    public static RepeatedField<string> ToRepeatedField(this List<Guid> guids)
    {
        var repeatedField = new RepeatedField<string>();
        foreach (var id in guids) repeatedField.Add(id.ToString());

        return repeatedField;
    }
    
    private static SprintDto ToDto(SprintProto proto, bool isActive)
    {
        var ticketIds = proto.TicketIds
            .Select(idStr => Guid.TryParse(idStr, out var guid) ? guid : Guid.Empty)
            .Where(guid => guid != Guid.Empty)
            .ToList();

        return new SprintDto(
            Guid.Parse(proto.SprintId),
            proto.Name,
            isActive,
            proto.Start.ToDateTimeOffset(),
            proto.End.ToDateTimeOffset(),
            ticketIds);
    }
}