using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.WorkDays;
using Client.Desktop.Communication.Requests.WorkDays.Records;
using Client.Desktop.DataModels;

namespace Client.Desktop.Communication.Mock.Requests;

public class MockWorkDayRequestSender(MockDataService mockDataService) : IWorkDayRequestSender
{
    public Task<List<WorkDayClientModel>> Send(ClientGetAllWorkDaysRequest request)
    {
        return Task.FromResult(mockDataService.WorkDays);
    }

    public Task<WorkDayClientModel> Send(ClientGetSelectedWorkDayRequest request)
    {
        return Task.FromResult(mockDataService.WorkDays.Find(wd => wd.WorkDayId == request.WorkDayId))!;
    }

    public Task<WorkDayClientModel> Send(ClientGetWorkDayByDateRequest request)
    {
        return Task.FromResult(mockDataService.WorkDays.First(wd => wd.Date.Date == request.Date.Date));
    }

    public Task<bool> Send(ClientIsWorkDayExistingRequest request)
    {
        return Task.FromResult(mockDataService.WorkDays.Any(wd => wd.Date.Date == request.Date.Date));
    }
}