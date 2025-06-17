
namespace Application.Contract.CQRS.Commands.Entities.Tags;

public record UpdateTagCommand(Guid TagId, string Name);