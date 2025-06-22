namespace Core.Server.Communication.Records.Commands.Entities.Tags;

public record UpdateTagCommand(Guid TagId, string Name);