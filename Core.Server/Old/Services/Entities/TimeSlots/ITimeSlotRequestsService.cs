namespace Service.Server.Old.Services.Entities.TimeSlots;

public interface ITimeSlotRequestsService
{
    Task<TimeSlotDto> GetById(Guid timeSlotId);
    Task<List<TimeSlotDto>> GetTimeSlotsForWorkDayId(Guid workDayId);
    Task<List<TimeSlotDto>> GetAll();
}