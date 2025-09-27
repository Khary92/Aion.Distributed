namespace Service.Admin.Web.Communication.Records.Commands;

public record WebCreateTagCommand(Guid TagId, string Name, Guid TraceId);