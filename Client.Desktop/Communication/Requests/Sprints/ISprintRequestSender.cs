using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Desktop.DTO;
using Proto.Requests.Sprints;

namespace Client.Desktop.Communication.Requests.Sprints;

public interface ISprintRequestSender
{
    Task<SprintDto?> Send(GetActiveSprintRequestProto request);
    Task<List<SprintDto>> Send(GetAllSprintsRequestProto request);
}