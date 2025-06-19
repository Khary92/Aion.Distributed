namespace Service.Server.Old.Services.Entities.WorkDays;

public interface IWorkDayRequestsService
{
    Task<List<WorkDayDto>> GetAll();
    Task<List<WorkDayDto>> GetWorkDaysInDateRange(DateTimeOffset startDate, DateTimeOffset endDate);
}