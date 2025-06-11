using System.Collections.Generic;
using System.Threading.Tasks;
using Contract.DTO;

namespace Client.Desktop.Communication.Requests.Sprints;

public interface ISprintRequestSender
{
    Task<SprintDto> GetActiveSprint();
    Task<List<SprintDto>> GetAllSprints();
}