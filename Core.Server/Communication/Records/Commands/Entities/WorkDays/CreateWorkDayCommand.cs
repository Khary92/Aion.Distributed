namespace Core.Server.Communication.Records.Commands.Entities.WorkDays;

public record CreateWorkDayCommand(
    Guid WorkDayId,
    DateTimeOffset Date);