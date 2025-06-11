using System.Threading.Tasks;
using Proto.Requests.Sprints;

namespace Client.Avalonia.Communication.Requests;

public interface ISprintRequestSender
{
    Task<SprintProto> GetActiveSprint();
    Task<SprintListProto> GetAllSprints();
}