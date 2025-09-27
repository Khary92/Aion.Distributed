namespace Service.Admin.Web.Communication.Records.Commands;

public record WebUpdateSprintDataCommand(
    Guid SprintId,
    string Name,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    Guid TraceId);