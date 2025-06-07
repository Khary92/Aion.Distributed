using MediatR;

namespace Contract.CQRS.Commands.Entities.WorkDays;

public record CreateWorkDayCommand(
    Guid WorkDayId,
    DateTimeOffset Date)
    : INotification, IRequest<Unit>;