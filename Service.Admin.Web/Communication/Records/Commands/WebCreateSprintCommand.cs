namespace Service.Admin.Web.Communication.Records.Commands;

public record WebCreateSprintCommand(
    Guid SprintId,
    string Name,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    bool IsActive,
    Guid TraceId);