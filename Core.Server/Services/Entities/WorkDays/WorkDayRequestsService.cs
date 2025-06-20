using Domain.Entities;
using Domain.Events.WorkDays;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.WorkDays;

public class WorkDayRequestsService(
    IEventStore<WorkDayEvent> workDayEventStore) : IWorkDayRequestsService
{
    public async Task<WorkDay?> GetById(Guid workDayId)
    {
        var allEvents = await workDayEventStore.GetAllEventsAsync();

        var groupedEvents = allEvents
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .ToList();

        return groupedEvents
            .Select(group => WorkDay.Rehydrate(group.ToList()))
            .ToList().FirstOrDefault(wd => wd.WorkDayId == workDayId);
    }

    public async Task<List<WorkDay>> GetAll()
    {
        var allEvents = await workDayEventStore.GetAllEventsAsync();

        var groupedEvents = allEvents
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .ToList();

        return groupedEvents
            .Select(group => WorkDay.Rehydrate(group.ToList()))
            .ToList();
    }

    public async Task<List<WorkDay>> GetWorkDaysInDateRange(DateTimeOffset startDate, DateTimeOffset endDate)
    {
        var allEvents = await workDayEventStore.GetAllEventsAsync();

        var groupedEvents = allEvents
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .ToList();

        return groupedEvents
            .Select(group => WorkDay.Rehydrate(group.ToList()))
            .Where(wd => wd.Date >= startDate && wd.Date <= endDate)
            .ToList();
    }

    public async Task<WorkDay?> GetWorkDayByDate(DateTimeOffset date)
    {
        var allEvents = await workDayEventStore.GetAllEventsAsync();

        var groupedEvents = allEvents
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .ToList();

        return groupedEvents
            .Select(group => WorkDay.Rehydrate(group.ToList()))
            .FirstOrDefault(wd => wd.Date.Date == date.Date);
    }

    public async Task<bool> IsWorkDayExisting(DateTimeOffset toDateTimeOffset)
    {
        var workDayByDate = await GetWorkDayByDate(toDateTimeOffset.Date.Date);
        return workDayByDate != null;
    }
}