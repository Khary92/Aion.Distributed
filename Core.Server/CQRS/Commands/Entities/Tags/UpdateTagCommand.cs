
namespace Service.Server.CQRS.Commands.Entities.Tags;

public record UpdateTagCommand(Guid TagId, string Name);