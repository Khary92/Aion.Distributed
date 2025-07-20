using Google.Protobuf.Collections;
using Proto.DTO.NoteType;
using Proto.DTO.Sprint;
using Proto.DTO.Tag;
using Proto.DTO.Ticket;
using Proto.Requests.NoteTypes;
using Proto.Requests.Sprints;
using Proto.Requests.Tags;
using Proto.Requests.Tickets;
using Service.Admin.Web.DTO;

namespace Service.Admin.Web.Communication;

public static class DtoExtensions
{
    public static List<NoteTypeDto> ToDtoList(this GetAllNoteTypesResponseProto? noteTypeListProto)
    {
        if (noteTypeListProto == null) return [];

        return noteTypeListProto.NoteTypes
            .Select(ToDto)
            .ToList();
    }

    private static NoteTypeDto ToDto(this NoteTypeProto noteType)
    {
        return new NoteTypeDto(Guid.Parse(noteType.NoteTypeId), noteType.Name, noteType.Color);
    }
    
    
    public static List<TagDto> ToDtoList(this TagListProto? tagListProto)
    {
        if (tagListProto == null) return [];

        return tagListProto.Tags
            .Select(ToDto)
            .ToList();
    }

    private static TagDto ToDto(this TagProto tag)
    {
        return new TagDto(Guid.Parse(tag.TagId), tag.Name, tag.IsSelected);
    }
    
    public static List<TicketDto> ToDtoList(this TicketListProto? ticketListProto)
    {
        if (ticketListProto == null) return [];

        return ticketListProto.Tickets
            .Select(ToDto)
            .ToList();
    }

    private static TicketDto ToDto(this TicketProto ticket)
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
    
    public static SprintDto ToDto(this SprintProto sprint)
    {
        var ticketIds = sprint.TicketIds
            .Select(idStr => Guid.TryParse(idStr, out var guid) ? guid : Guid.Empty)
            .Where(guid => guid != Guid.Empty)
            .ToList();

        return new SprintDto(
            Guid.Parse(sprint.SprintId),
            sprint.Name,
            sprint.IsActive,
            sprint.Start.ToDateTimeOffset(),
            sprint.End.ToDateTimeOffset(),
            sprint.TicketIds.ToGuidList()
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
    
    public static List<SprintDto> ToDtoList(this SprintListProto? sprintListProto)
    {
        if (sprintListProto == null) return [];

        return sprintListProto.Sprints
            .Select(ToDto)
            .ToList();
    }
}