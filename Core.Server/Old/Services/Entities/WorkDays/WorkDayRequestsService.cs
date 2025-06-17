using Application.Contract.DTO;
using Application.Mapper;
using Domain.Entities;
using Domain.Events.WorkDays;
using Domain.Interfaces;

namespace Application.Services.Entities.WorkDays;

public class WorkDayRequestsService(
    IEventStore<WorkDayEvent> workDayEventStore,
    IDtoMapper<WorkDayDto, WorkDay> workDayMapper) : IWorkDayRequestsService
{
    public async Task<List<WorkDayDto>> GetAll()
    {
        var allEvents = await workDayEventStore.GetAllEventsAsync();

        var groupedEvents = allEvents
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .ToList();

        return groupedEvents
            .Select(group => WorkDay.Rehydrate(group.ToList()))
            .Select(workDayMapper.ToDto)
            .ToList();
    }


    public async Task<List<WorkDayDto>> GetWorkDaysInDateRange(DateTimeOffset startDate, DateTimeOffset endDate)
    {
        var allEvents = await workDayEventStore.GetAllEventsAsync();

        var groupedEvents = allEvents
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .ToList();

        return groupedEvents
            .Select(group => WorkDay.Rehydrate(group.ToList()))
            .Select(workDayMapper.ToDto)
            .Where(wd => wd.Date >= startDate && wd.Date <= endDate)
            .ToList();
    }
}