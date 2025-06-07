using Contract.DTO;
using MediatR;

namespace Contract.CQRS.Requests.WorkDays;

public record GetWorkDayByDateRequest(DateTimeOffset Date) : IRequest<WorkDayDto?>, INotification;