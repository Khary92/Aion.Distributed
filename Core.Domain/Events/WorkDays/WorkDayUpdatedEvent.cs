namespace Domain.Events.WorkDays;

public record WorkDayUpdatedEvent(
    Guid WorkDayId,
    DateTimeOffset Date);