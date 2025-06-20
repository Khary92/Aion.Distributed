using Domain.Entities;

namespace Core.Server.Services.Entities.TimeSlots;

public interface ITimeSlotRequestsService
{
    Task<TimeSlot> GetById(Guid timeSlotId);
    Task<List<TimeSlot>> GetTimeSlotsForWorkDayId(Guid workDayId);
    Task<List<TimeSlot>> GetAll();
}