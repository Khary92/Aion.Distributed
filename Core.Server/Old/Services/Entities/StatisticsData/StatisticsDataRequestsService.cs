using Application.Contract.DTO;
using Application.Mapper;
using Domain.Events.StatisticsData;
using Domain.Interfaces;

namespace Application.Services.Entities.StatisticsData;

public class StatisticsDataRequestsService(
    IEventStore<StatisticsDataEvent> statisticsDataEventStore,
    IDtoMapper<StatisticsDataDto, Domain.Entities.StatisticsData> statisticsDataMapper)
    : IStatisticsDataRequestsService
{
    public async Task<StatisticsDataDto> GetStatisticsDataByTimeSlotId(Guid timeSlotId)
    {
        var allStatisticsData = await GetAll();

        return allStatisticsData.First(x => x.TimeSlotId == timeSlotId);
    }

    public async Task<List<StatisticsDataDto>> GetAll()
    {
        var allEvents = await statisticsDataEventStore.GetAllEventsAsync();

        var groupedEvents = allEvents
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .ToList();

        return groupedEvents
            .Select(group => Domain.Entities.StatisticsData.Rehydrate(group.ToList()))
            .Select(statisticsDataMapper.ToDto)
            .ToList();
    }
}