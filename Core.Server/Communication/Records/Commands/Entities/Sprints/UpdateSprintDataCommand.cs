namespace Core.Server.Communication.Records.Commands.Entities.Sprints;

public record UpdateSprintDataCommand(
    Guid SprintId,
    string Name,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime, Guid TraceId);