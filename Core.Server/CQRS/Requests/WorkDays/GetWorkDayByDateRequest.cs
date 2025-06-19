
namespace Service.Server.CQRS.Requests.WorkDays;

public record GetWorkDayByDateRequest(DateTimeOffset Date);