using MediatR;

namespace Contract.CQRS.Commands.Entities.Tags;

public record UpdateTagCommand(Guid TagId, string Name)
    : INotification, IRequest<Unit>;