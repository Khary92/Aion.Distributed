using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.TimeSlots;
using Client.Desktop.Communication.Requests.TimeSlots.Records;
using Client.Desktop.DataModels;

namespace Client.Desktop.Communication.Mock.Requests;

public class MockTimeSlotRequestSender(MockDataService mockDataService) : ITimeSlotRequestSender
{
    public Task<TimeSlotClientModel> Send(ClientGetTimeSlotByIdRequest request)
    {
        return Task.FromResult(mockDataService.TimeSlots.First(ts => ts.TimeSlotId == request.TimeSlotId));
    }

    public Task<List<TimeSlotClientModel>> Send(ClientGetTimeSlotsForWorkDayIdRequest request)
    {
        return Task.FromResult(mockDataService.TimeSlots.Where(ts => ts.WorkDayId == request.WorkDayId).ToList());
    }
}