using System.Collections.Generic;
using System.Threading.Tasks;
using Contract.DTO;

namespace Client.Desktop.Communication.Requests.TimeSlots;

public interface ITimeSlotRequestSender
{
    Task<TimeSlotDto> GetTimeSlotById(string timeSlotId);
    Task<List<TimeSlotDto>> GetTimeSlotsForWorkDayId(string workDayId);
}