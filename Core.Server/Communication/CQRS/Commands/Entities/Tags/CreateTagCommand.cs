
namespace Service.Server.Communication.CQRS.Commands.Entities.Tags;

public record CreateTagCommand(Guid TagId, string Name);