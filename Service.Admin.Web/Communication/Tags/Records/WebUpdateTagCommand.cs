namespace Service.Admin.Web.Communication.Tags.Records;

public record WebUpdateTagCommand(Guid TagId, string Name, Guid TraceId);