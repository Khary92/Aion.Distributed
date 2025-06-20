using Domain.Entities;

namespace Service.Server.Old.Services.Entities.WorkDays;

public interface IWorkDayRequestsService
{
    Task<WorkDay?> GetById(Guid workDayId);
    Task<List<WorkDay>> GetAll();
    Task<List<WorkDay>> GetWorkDaysInDateRange(DateTimeOffset startDate, DateTimeOffset endDate);
    Task<WorkDay?> GetWorkDayByDate(DateTimeOffset date);
}