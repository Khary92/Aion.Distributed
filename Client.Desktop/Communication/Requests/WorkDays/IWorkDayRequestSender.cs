using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Proto.Requests.WorkDays;

namespace Client.Desktop.Communication.Requests.WorkDays;

public interface IWorkDayRequestSender
{
    Task<List<WorkDayClientModel>> Send(GetAllWorkDaysRequestProto request);
    Task<WorkDayClientModel> Send(GetSelectedWorkDayRequestProto request);
    Task<WorkDayClientModel> Send(GetWorkDayByDateRequestProto request);
    Task<bool> Send(IsWorkDayExistingRequestProto request);
}