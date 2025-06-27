using System;
using Client.Desktop.DTO;
using Client.Desktop.Proto;
using Proto.DTO.AiSettings;
using Proto.DTO.StatisticsData;
using Proto.DTO.Ticket;
using Proto.DTO.TimeSlots;

namespace Client.Desktop.Communication.Requests;

public static class ProtoToDtoMapper
{
    public static AiSettingsDto ToDto(this AiSettingsProto proto)
    {
        return new AiSettingsDto(Guid.Parse(proto.AiSettingsId), proto.LanguageModelPath, proto.Prompt);
    }

    public static TicketDto ToDto(this TicketProto proto)
    {
        return new TicketDto(Guid.Parse(proto.TicketId), proto.Name, proto.BookingNumber, proto.Documentation,
            proto.SprintIds.ToGuidList());
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