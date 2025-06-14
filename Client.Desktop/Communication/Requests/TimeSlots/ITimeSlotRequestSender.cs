using System.Collections.Generic;
using System.Threading.Tasks;
using Contract.DTO;
using Proto.Requests.TimeSlots;

namespace Client.Desktop.Communication.Requests.TimeSlots;

public interface ITimeSlotRequestSender
{
    Task<TimeSlotDto> Send(GetTimeSlotByIdRequestProto request);
    Task<List<TimeSlotDto>> Send(GetTimeSlotsForWorkDayIdRequestProto request);
}