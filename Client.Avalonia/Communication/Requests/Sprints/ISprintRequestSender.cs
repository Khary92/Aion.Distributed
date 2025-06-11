using System.Threading.Tasks;
using Proto.Requests.Sprints;

namespace Client.Avalonia.Communication.Requests.Sprints;

public interface ISprintRequestSender
{
    Task<SprintProto> GetActiveSprint();
    Task<SprintListProto> GetAllSprints();
}