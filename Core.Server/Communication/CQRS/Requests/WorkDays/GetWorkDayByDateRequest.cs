namespace Core.Server.Communication.CQRS.Requests.WorkDays;

public record GetWorkDayByDateRequest(DateTimeOffset Date);