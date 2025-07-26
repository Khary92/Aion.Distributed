using System;
using System.Collections.Generic;
using System.Linq;
using Client.Desktop.DataModels;
using Client.Proto;
using Proto.DTO.NoteType;
using Proto.DTO.Sprint;
using Proto.DTO.StatisticsData;
using Proto.DTO.Tag;
using Proto.DTO.Ticket;
using Proto.DTO.TimerSettings;
using Proto.DTO.TimeSlots;
using Proto.Requests.NoteTypes;
using Proto.Requests.Sprints;
using Proto.Requests.Tags;
using Proto.Requests.Tickets;

namespace Client.Desktop.Communication.Requests;

public static class DtoExtensions
{
    public static List<NoteTypeClientModel> ToModelList(this GetAllNoteTypesResponseProto? noteTypeListProto)
    {
        return noteTypeListProto == null ? [] : noteTypeListProto.NoteTypes.Select(ToModel).ToList();
    }

    public static NoteTypeClientModel ToModel(this NoteTypeProto noteType)
    {
        return new NoteTypeClientModel(Guid.Parse(noteType.NoteTypeId), noteType.Name, noteType.Color);
    }

    public static List<TagClientModel> ToModelList(this TagListProto? tagListProto)
    {
        if (tagListProto == null) return [];

        return tagListProto.Tags
            .Select(ToModel)
            .ToList();
    }

    public static TagClientModel ToModel(this TagProto tag)
    {
        return new TagClientModel(Guid.Parse(tag.TagId), tag.Name, tag.IsSelected);
    }

    public static List<TicketClientModel> ToModelList(this TicketListProto? ticketListProto)
    {
        if (ticketListProto == null) return [];

        return ticketListProto.Tickets
            .Select(ToModel)
            .ToList();
    }

    public static TicketClientModel ToModel(this TicketProto ticket)
    {
        var sprintIds = ticket.SprintIds
            .Select(idStr => Guid.TryParse(idStr, out var guid) ? guid : Guid.Empty)
            .Where(guid => guid != Guid.Empty)
            .ToList();

        return new TicketClientModel(
            Guid.Parse(ticket.TicketId),
            ticket.Name,
            ticket.BookingNumber,
            ticket.Documentation,
            [..sprintIds]
        );
    }

    public static List<SprintClientModel?> ToModelList(this SprintListProto? sprintListProto)
    {
        if (sprintListProto == null) return [];

        return sprintListProto.Sprints
            .Select(ToModel)
            .ToList();
    }

    public static SprintClientModel? ToModel(this SprintProto? sprint)
    {
        if (sprint == null) return null;
        var ticketIds = sprint.TicketIds
            .Select(idStr => Guid.TryParse(idStr, out var guid) ? guid : Guid.Empty)
            .Where(guid => guid != Guid.Empty)
            .ToList();

        return new SprintClientModel(
            Guid.Parse(sprint.SprintId),
            sprint.Name,
            sprint.IsActive,
            sprint.Start.ToDateTimeOffset(),
            sprint.End.ToDateTimeOffset(),
            [..ticketIds]
        );
    }

    public static StatisticsDataClientModel ToModel(this StatisticsDataProto proto)
    {
        return new StatisticsDataClientModel(Guid.Parse(proto.StatisticsId), Guid.Parse(proto.TimeSlotId),
            proto.TagIds.ToGuidList(), proto.IsProductive, proto.IsNeutral, proto.IsUnproductive);
    }

    public static TimeSlotClientModel ToModel(this TimeSlotProto proto)
    {
        return new TimeSlotClientModel(Guid.Parse(proto.TimeSlotId), Guid.Parse(proto.WorkDayId),
            Guid.Parse(proto.SelectedTicketId), proto.StartTime.ToDateTimeOffset(), proto.EndTime.ToDateTimeOffset(),
            proto.NoteIds.ToGuidList(), proto.IsTimerRunning);
    }
}