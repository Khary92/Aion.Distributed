namespace Service.Admin.Web.Communication.Sprints.Records;

public record WebSetSprintActiveStatusCommand(Guid SprintId, bool IsActive, Guid TraceId);