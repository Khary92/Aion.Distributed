using Domain.Entities;

namespace Service.Server.Old.Services.Entities.TimeSlots;

public interface ITimeSlotRequestsService
{
    Task<TimeSlot> GetById(Guid timeSlotId);
    Task<List<TimeSlot>> GetTimeSlotsForWorkDayId(Guid workDayId);
    Task<List<TimeSlot>> GetAll();
}