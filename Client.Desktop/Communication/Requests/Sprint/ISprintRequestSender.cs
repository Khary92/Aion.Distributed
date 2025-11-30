using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.DataModels;
using Proto.Requests.Sprints;

namespace Client.Desktop.Communication.Requests.Sprint;

public interface ISprintRequestSender
{
    Task<SprintClientModel?> Send(GetActiveSprintRequestProto request);
    Task<List<SprintClientModel>> Send(GetAllSprintsRequestProto request);
}