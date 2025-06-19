using Google.Protobuf.WellKnownTypes;
using Proto.DTO.TimeSlots;

namespace Service.Server.Communication.Mapper;

public class TimeSlotMapper : IDtoMapper<TimeSlotProto, Domain.Entities.TimeSlot>
{
    public Domain.Entities.TimeSlot ToDomain(TimeSlotProto dto)
    {
        return new Domain.Entities.TimeSlot
        {
            TimeSlotId = Guid.Parse(dto.TimeSlotId),
            SelectedTicketId = Guid.Parse(dto.SelectedTicketId),
            StartTime = dto.StartTime.ToDateTimeOffset(),
            EndTime = dto.EndTime.ToDateTimeOffset(),
            WorkDayId = Guid.Parse(dto.WorkDayId),
            IsTimerRunning = dto.IsTimerRunning
        };
    }

    public TimeSlotProto ToDto(Domain.Entities.TimeSlot domain)
    {
        var timeSlotDto = new TimeSlotProto
        {
            TimeSlotId = domain.TimeSlotId.ToString(),
            SelectedTicketId = domain.SelectedTicketId.ToString(),
            StartTime = Timestamp.FromDateTimeOffset(domain.StartTime),
            EndTime = Timestamp.FromDateTimeOffset(domain.EndTime),
            WorkDayId = domain.WorkDayId.ToString(),
            IsTimerRunning = domain.IsTimerRunning,
            NoteIds = { domain.NoteIds.ToRepeatedField() }
        };

        return timeSlotDto;
    }
}