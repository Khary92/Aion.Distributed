using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.TimeSlots.Records;
using Client.Desktop.DataModels;
using Proto.Requests.TimeSlots;

namespace Client.Desktop.Communication.Requests.TimeSlots;

public interface ITimeSlotRequestSender
{
    Task<TimeSlotClientModel> Send(ClientGetTimeSlotByIdRequest request);
    Task<List<TimeSlotClientModel>> Send(ClientGetTimeSlotsForWorkDayIdRequest request);
}