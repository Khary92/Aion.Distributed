namespace Service.Admin.Web.Communication.Records.Commands;

public record WebSetSprintActiveStatusCommand(Guid SprintId, bool IsActive, Guid TraceId);