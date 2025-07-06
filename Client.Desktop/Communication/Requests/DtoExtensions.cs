using System;
using System.Collections.Generic;
using System.Linq;
using Client.Desktop.DTO;
using Client.Proto;
using Proto.DTO.AiSettings;
using Proto.DTO.StatisticsData;
using Proto.DTO.Ticket;
using Proto.DTO.TimeSlots;
using Proto.Requests.Tickets;

namespace Client.Desktop.Communication.Requests;

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
    
    public static AiSettingsDto ToDto(this AiSettingsProto proto)
    {
        return new AiSettingsDto(Guid.Parse(proto.AiSettingsId), proto.LanguageModelPath, proto.Prompt);
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