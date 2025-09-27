namespace Service.Admin.Web.Communication.Records.Commands;

public record WebUpdateTagCommand(Guid TagId, string Name, Guid TraceId);