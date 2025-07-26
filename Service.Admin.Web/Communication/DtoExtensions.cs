using Google.Protobuf.Collections;
using Proto.DTO.NoteType;
using Proto.DTO.Sprint;
using Proto.DTO.Tag;
using Proto.DTO.Ticket;
using Proto.DTO.TimerSettings;
using Proto.Requests.NoteTypes;
using Proto.Requests.Sprints;
using Proto.Requests.Tags;
using Proto.Requests.Tickets;
using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication;

public static class DtoExtensions
{
    public static TimerSettingsWebModel ToDto(this TimerSettingsProto timerSettings)
    {
        return new TimerSettingsWebModel(Guid.Parse(timerSettings.TimerSettingsId), timerSettings.DocumentationSaveInterval,
            timerSettings.SnapshotSaveInterval);
    }

    public static List<NoteTypeWebModel> ToDtoList(this GetAllNoteTypesResponseProto? noteTypeListProto)
    {
        if (noteTypeListProto == null) return [];

        return noteTypeListProto.NoteTypes
            .Select(ToDto)
            .ToList();
    }

    private static NoteTypeWebModel ToDto(this NoteTypeProto noteType)
    {
        return new NoteTypeWebModel(Guid.Parse(noteType.NoteTypeId), noteType.Name, noteType.Color);
    }

    public static List<TagWebModel> ToDtoList(this TagListProto? tagListProto)
    {
        if (tagListProto == null) return [];

        return tagListProto.Tags
            .Select(ToDto)
            .ToList();
    }

    private static TagWebModel ToDto(this TagProto tag)
    {
        return new TagWebModel(Guid.Parse(tag.TagId), tag.Name, tag.IsSelected);
    }

    public static List<TicketWebModel> ToDtoList(this TicketListProto? ticketListProto)
    {
        if (ticketListProto == null) return [];

        return ticketListProto.Tickets
            .Select(ToDto)
            .ToList();
    }

    private static TicketWebModel ToDto(this TicketProto ticket)
    {
        var sprintIds = ticket.SprintIds
            .Select(idStr => Guid.TryParse(idStr, out var guid) ? guid : Guid.Empty)
            .Where(guid => guid != Guid.Empty)
            .ToList();

        return new TicketWebModel(
            Guid.Parse(ticket.TicketId),
            ticket.Name,
            ticket.BookingNumber,
            ticket.Documentation,
            [..sprintIds]
        );
    }

    public static SprintWebModel ToDto(this SprintProto sprint)
    {
        var ticketIds = sprint.TicketIds
            .Select(idStr => Guid.TryParse(idStr, out var guid) ? guid : Guid.Empty)
            .Where(guid => guid != Guid.Empty)
            .ToList();

        return new SprintWebModel(
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

    private static SprintWebModel ToDto(SprintProto proto, bool isActive)
    {
        var ticketIds = proto.TicketIds
            .Select(idStr => Guid.TryParse(idStr, out var guid) ? guid : Guid.Empty)
            .Where(guid => guid != Guid.Empty)
            .ToList();

        return new SprintWebModel(
            Guid.Parse(proto.SprintId),
            proto.Name,
            isActive,
            proto.Start.ToDateTimeOffset(),
            proto.End.ToDateTimeOffset(),
            ticketIds);
    }

    public static List<SprintWebModel> ToDtoList(this SprintListProto? sprintListProto)
    {
        if (sprintListProto == null) return [];

        return sprintListProto.Sprints
            .Select(ToDto)
            .ToList();
    }
}