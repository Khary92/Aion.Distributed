using Contract.DTO;
using MediatR;

namespace Contract.CQRS.Requests.WorkDays;

public record GetSelectedWorkDayRequest : IRequest<WorkDayDto>, INotification;