using Application.Contract.CQRS.Requests.Sprints;
using Application.Contract.DTO;
using Application.Services.Entities.Sprints;
using MediatR;

namespace Application.Handler.Requests.Sprints;

public class GetAllSprintsRequestHandler(ISprintRequestsService sprintRequestsService)
    : IRequestHandler<GetAllSprintsRequest, List<SprintDto>>
{
    public async Task<List<SprintDto>> Handle(GetAllSprintsRequest request, CancellationToken cancellationToken)
    {
        return await sprintRequestsService.GetAll();
    }
}