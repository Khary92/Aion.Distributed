using Service.Monitoring.Shared;

namespace Service.Admin.Web.Communication.Sprints.Records;

public record WebCreateSprintCommand(
    Guid SprintId,
    string Name,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    bool IsActive,
    List<Guid> TicketIds,
    Guid TraceId);