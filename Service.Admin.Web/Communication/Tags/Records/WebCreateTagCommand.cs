namespace Service.Admin.Web.Communication.Tags.Records;

public record WebCreateTagCommand(Guid TagId, string Name, Guid TraceId);