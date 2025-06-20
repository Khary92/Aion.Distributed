
namespace Service.Server.Communication.CQRS.Commands.Entities.Tags;

public record UpdateTagCommand(Guid TagId, string Name);