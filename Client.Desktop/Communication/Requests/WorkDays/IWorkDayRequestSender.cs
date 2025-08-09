using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.WorkDays.Records;
using Client.Desktop.DataModels;

namespace Client.Desktop.Communication.Requests.WorkDays;

public interface IWorkDayRequestSender
{
    Task<List<WorkDayClientModel>> Send(ClientGetAllWorkDaysRequest request);
    Task<WorkDayClientModel> Send(ClientGetSelectedWorkDayRequest request);
    Task<WorkDayClientModel> Send(ClientGetWorkDayByDateRequest request);
    Task<bool> Send(ClientIsWorkDayExistingRequest request);
}