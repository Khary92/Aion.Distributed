using Domain.Entities;
using Domain.Events.TimeSlots;
using Domain.Interfaces;

namespace Core.Server.Services.Entities.TimeSlots;

public class TimeSlotRequestsService(IEventStore<TimeSlotEvent> timeSlotEventsStore) : ITimeSlotRequestsService
{
    public async Task<TimeSlot> GetById(Guid timeSlotId)
    {
        var timeSlotEvents = await timeSlotEventsStore
            .GetEventsForAggregateAsync(timeSlotId);

        return TimeSlot.Rehydrate(timeSlotEvents);
    }

    public async Task<List<TimeSlot>> GetTimeSlotsForWorkDayId(Guid workDayId)
    {
        var timeSlotDtos = await GetAll();
        return timeSlotDtos.Where(x => x.WorkDayId == workDayId).ToList();
    }

    public async Task<List<TimeSlot>> GetAll()
    {
        var allEvents = await timeSlotEventsStore.GetAllEventsAsync();

        var groupedEvents = allEvents
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .ToList();

        return groupedEvents
            .Select(group => TimeSlot.Rehydrate(group.ToList()))
            .ToList();
    }
}