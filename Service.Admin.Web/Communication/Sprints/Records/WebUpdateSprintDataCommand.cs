namespace Service.Admin.Web.Communication.Sprints.Records;

public record WebUpdateSprintDataCommand(
    Guid SprintId,
    string Name,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    Guid TraceId);