using System.Collections.Generic;
using System.Threading.Tasks;
using Contract.DTO;
using Proto.Requests.Sprints;

namespace Client.Avalonia.Communication.Requests.Sprints;

public interface ISprintRequestSender
{
    Task<SprintDto> GetActiveSprint();
    Task<List<SprintDto>> GetAllSprints();
}