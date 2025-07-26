using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Proto.Requests.TimeSlots;

namespace Client.Desktop.Communication.Requests.TimeSlots;

public interface ITimeSlotRequestSender
{
    Task<TimeSlotClientModel> Send(GetTimeSlotByIdRequestProto request);
    Task<List<TimeSlotClientModel>> Send(GetTimeSlotsForWorkDayIdRequestProto request);
}