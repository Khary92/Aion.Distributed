using Contract.DTO;
using MediatR;

namespace Contract.CQRS.Requests.WorkDays;

public record GetAllWorkDaysRequest : IRequest<List<WorkDayDto>>, INotification;