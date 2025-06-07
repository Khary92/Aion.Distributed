using MediatR;

namespace Contract.CQRS.Commands.Entities.Tags;

public record CreateTagCommand(Guid TagId, string Name)
    : INotification, IRequest<Unit>;