using System;
using System.Collections.Generic;
using System.Linq;
using Client.Desktop.DTO;
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
    public static TimerSettingsDto ToDto(this TimerSettingsProto timerSettings)
    {
        return new TimerSettingsDto(Guid.Parse(timerSettings.TimerSettingsId), timerSettings.DocumentationSaveInterval,
            timerSettings.SnapshotSaveInterval);
    }

    public static List<NoteTypeDto> ToDtoList(this GetAllNoteTypesResponseProto? noteTypeListProto)
    {
        return noteTypeListProto == null ? [] : noteTypeListProto.NoteTypes.Select(ToDto).ToList();
    }

    public static NoteTypeDto ToDto(this NoteTypeProto noteType)
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

    public static TagDto ToDto(this TagProto tag)
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

    public static List<SprintDto?> ToDtoList(this SprintListProto? sprintListProto)
    {
        if (sprintListProto == null) return [];

        return sprintListProto.Sprints
            .Select(ToDto)
            .ToList();
    }

    public static SprintDto? ToDto(this SprintProto? sprint)
    {
        if (sprint == null) return null;
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
            [..ticketIds]
        );
    }

    public static StatisticsDataDto ToDto(this StatisticsDataProto proto)
    {
        return new StatisticsDataDto(Guid.Parse(proto.StatisticsId), Guid.Parse(proto.TimeSlotId),
            proto.TagIds.ToGuidList(), proto.IsProductive, proto.IsNeutral, proto.IsUnproductive);
    }

    public static TimeSlotDto ToDto(this TimeSlotProto proto)
    {
        return new TimeSlotDto(Guid.Parse(proto.TimeSlotId), Guid.Parse(proto.WorkDayId),
            Guid.Parse(proto.SelectedTicketId), proto.StartTime.ToDateTimeOffset(), proto.EndTime.ToDateTimeOffset(),
            proto.NoteIds.ToGuidList(), proto.IsTimerRunning);
    }
}