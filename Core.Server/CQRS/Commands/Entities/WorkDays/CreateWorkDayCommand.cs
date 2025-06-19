
namespace Service.Server.CQRS.Commands.Entities.WorkDays;

public record CreateWorkDayCommand(
    Guid WorkDayId,
    DateTimeOffset Date);