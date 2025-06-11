using System.Threading.Tasks;
using Proto.Requests.TimeSlots;

namespace Client.Avalonia.Communication.Requests.TimeSlots;

public interface ITimeSlotRequestSender
{
    Task<TimeSlotProto> GetTimeSlotById(string timeSlotId);
    Task<TimeSlotListProto> GetTimeSlotsForWorkDayId(string workDayId);
}