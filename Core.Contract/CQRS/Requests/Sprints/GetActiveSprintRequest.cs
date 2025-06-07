using Contract.DTO;
using MediatR;

namespace Contract.CQRS.Requests.Sprints;

public class GetActiveSprintRequest : IRequest<SprintDto?>, INotification;