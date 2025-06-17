
namespace Application.Contract.CQRS.Requests.WorkDays;

public record GetWorkDayByDateRequest(DateTimeOffset Date);