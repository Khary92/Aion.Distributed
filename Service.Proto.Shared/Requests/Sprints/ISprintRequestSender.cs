using Proto.DTO.Sprint;
using Proto.Requests.Sprints;

namespace Service.Proto.Shared.Requests.Sprints;

public interface ISprintRequestSender
{
    Task<SprintProto?> Send(GetActiveSprintRequestProto request);
    Task<SprintListProto> Send(GetAllSprintsRequestProto request);
}