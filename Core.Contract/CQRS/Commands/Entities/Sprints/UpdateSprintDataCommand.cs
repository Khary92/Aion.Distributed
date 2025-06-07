using MediatR;

namespace Contract.CQRS.Commands.Entities.Sprints;

public record UpdateSprintDataCommand(
    Guid SprintId,
    string Name,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime)
    : IRequest<Unit>, INotification;