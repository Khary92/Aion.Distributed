using Domain.Entities;

namespace Core.Server.Services.Entities.WorkDays;

public interface IWorkDayRequestsService
{
    Task<WorkDay?> GetById(Guid workDayId);
    Task<List<WorkDay>> GetAll();
    Task<List<WorkDay>> GetWorkDaysInDateRange(DateTimeOffset startDate, DateTimeOffset endDate);
    Task<WorkDay?> GetWorkDayByDate(DateTimeOffset date);
    Task<bool> IsWorkDayExisting(DateTimeOffset toDateTimeOffset);
}