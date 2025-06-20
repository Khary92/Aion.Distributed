namespace Core.Server.Communication.CQRS.Commands.Entities.WorkDays;

public record CreateWorkDayCommand(
    Guid WorkDayId,
    DateTimeOffset Date);