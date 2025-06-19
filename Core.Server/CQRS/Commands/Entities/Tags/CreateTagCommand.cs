
namespace Service.Server.CQRS.Commands.Entities.Tags;

public record CreateTagCommand(Guid TagId, string Name);