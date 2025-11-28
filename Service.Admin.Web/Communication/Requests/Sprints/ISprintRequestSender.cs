using Proto.DTO.Sprint;
using Proto.Requests.Sprints;

namespace Service.Admin.Web.Communication.Requests.Sprints;

public interface ISprintRequestSender
{
    Task<SprintProto?> Send(GetActiveSprintRequestProto request);
    Task<SprintListProto> Send(GetAllSprintsRequestProto request);
}