using Application.Contract.DTO;
using Domain.Entities;

namespace Application.Mapper;

public class TimeSlotMapper : IDtoMapper<TimeSlotDto, TimeSlot>
{
    public TimeSlot ToDomain(TimeSlotDto dto)
    {
        return new TimeSlot
        {
            TimeSlotId = dto.TimeSlotId,
            SelectedTicketId = dto.SelectedTicketId,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            WorkDayId = dto.WorkDayId,
            IsTimerRunning = dto.IsTimerRunning
        };
    }

    public TimeSlotDto ToDto(TimeSlot domain)
    {
        var timeSlotDto = new TimeSlotDto(domain.TimeSlotId, domain.WorkDayId, domain.SelectedTicketId,
            domain.StartTime, domain.EndTime, domain.NoteIds, domain.IsTimerRunning);

        return timeSlotDto;
    }
}