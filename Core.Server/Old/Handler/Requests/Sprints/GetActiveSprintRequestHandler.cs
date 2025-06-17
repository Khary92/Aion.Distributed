using Application.Contract.CQRS.Requests.Sprints;
using Application.Contract.DTO;
using Application.Services.Entities.Sprints;
using MediatR;

namespace Application.Handler.Requests.Sprints;

public class GetActiveSprintRequestHandler(ISprintRequestsService sprintRequestsService) :
    IRequestHandler<GetActiveSprintRequest, SprintDto?>
{
    public async Task<SprintDto?> Handle(GetActiveSprintRequest request, CancellationToken cancellationToken)
    {
        return (await sprintRequestsService.GetAll()).FirstOrDefault(s => s.IsActive);
    }
}