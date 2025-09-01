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
    public static TimerSettingsWebModel ToWebModel(this TimerSettingsProto timerSettings)
    {
        return new TimerSettingsWebModel(Guid.Parse(timerSettings.TimerSettingsId),
            timerSettings.DocumentationSaveInterval,
            timerSettings.SnapshotSaveInterval);
    }

    public static List<NoteTypeWebModel> ToWebModelList(this GetAllNoteTypesResponseProto? noteTypeListProto)
    {
        if (noteTypeListProto == null) return [];

        return noteTypeListProto.NoteTypes
            .Select(ToWebModel)
            .ToList();
    }

    private static NoteTypeWebModel ToWebModel(this NoteTypeProto noteType)
    {
        return new NoteTypeWebModel(Guid.Parse(noteType.NoteTypeId), noteType.Name, noteType.Color);
    }

    public static List<TagWebModel> ToWebModelList(this TagListProto? tagListProto)
    {
        if (tagListProto == null) return [];

        return tagListProto.Tags
            .Select(ToWebModel)
            .ToList();
    }

    private static TagWebModel ToWebModel(this TagProto tag)
    {
        return new TagWebModel(Guid.Parse(tag.TagId), tag.Name);
    }

    public static List<TicketWebModel> ToWebModelList(this TicketListProto? ticketListProto)
    {
        if (ticketListProto == null) return [];

        return ticketListProto.Tickets
            .Select(ToWebModel)
            .ToList();
    }

    private static TicketWebModel ToWebModel(this TicketProto ticket)
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

    private static SprintWebModel ToWebModel(this SprintProto sprint)
    {
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

    public static List<SprintWebModel> ToWebModelList(this SprintListProto? sprintListProto)
    {
        if (sprintListProto == null) return [];

        return sprintListProto.Sprints
            .Select(ToWebModel)
            .ToList();
    }
}