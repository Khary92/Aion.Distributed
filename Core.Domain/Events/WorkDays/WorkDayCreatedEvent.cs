namespace Domain.Events.WorkDays;

public record WorkDayCreatedEvent(
    Guid WorkDayId,
    DateTimeOffset Date);