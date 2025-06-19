using Domain.Entities;
using Domain.Events.TimeSlots;
using Domain.Interfaces;
using Service.Server.Communication.Mapper;

namespace Service.Server.Old.Services.Entities.TimeSlots;

public class TimeSlotRequestsService(
    IEventStore<TimeSlotEvent> timeSlotEventsStore,
    IDtoMapper<TimeSlotDto, TimeSlot> timeSlotMapper) : ITimeSlotRequestsService
{
    public async Task<TimeSlotDto> GetById(Guid timeSlotId)
    {
        var timeSlotEvents = await timeSlotEventsStore
            .GetEventsForAggregateAsync(timeSlotId);

        var domainTimeSlot = TimeSlot.Rehydrate(timeSlotEvents);
        return timeSlotMapper.ToDto(domainTimeSlot);
    }

    public async Task<List<TimeSlotDto>> GetTimeSlotsForWorkDayId(Guid workDayId)
    {
        var timeSlotDtos = await GetAll();
        return timeSlotDtos.Where(x => x.WorkDayId == workDayId).ToList();
    }

    public async Task<List<TimeSlotDto>> GetAll()
    {
        var allEvents = await timeSlotEventsStore.GetAllEventsAsync();

        var groupedEvents = allEvents
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .ToList();

        return groupedEvents
            .Select(group => TimeSlot.Rehydrate(group.ToList()))
            .Select(timeSlotMapper.ToDto)
            .ToList();
    }
}