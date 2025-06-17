
namespace Application.Contract.CQRS.Commands.Entities.Tags;

public record CreateTagCommand(Guid TagId, string Name);