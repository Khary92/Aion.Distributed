
namespace Service.Server.Communication.CQRS.Requests.WorkDays;

public record GetWorkDayByDateRequest(DateTimeOffset Date);