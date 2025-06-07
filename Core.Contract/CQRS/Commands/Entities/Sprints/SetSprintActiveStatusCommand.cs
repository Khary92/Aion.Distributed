using MediatR;

namespace Contract.CQRS.Commands.Entities.Sprints;

public record SetSprintActiveStatusCommand(Guid SprintId, bool IsActive)
    : IRequest<Unit>, INotification;