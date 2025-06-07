using Contract.DTO;
using MediatR;

namespace Contract.CQRS.Requests.Sprints;

public record GetAllSprintsRequest : IRequest<List<SprintDto>>, INotification;