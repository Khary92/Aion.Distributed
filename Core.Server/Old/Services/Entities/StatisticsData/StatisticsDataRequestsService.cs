using Domain.Events.StatisticsData;
using Domain.Interfaces;
using Service.Server.Communication.Mapper;

namespace Service.Server.Old.Services.Entities.StatisticsData;

public class StatisticsDataRequestsService(IEventStore<StatisticsDataEvent> statisticsDataEventStore)
    : IStatisticsDataRequestsService
{
    public async Task<Domain.Entities.StatisticsData> GetStatisticsDataByTimeSlotId(Guid timeSlotId)
    {
        var allStatisticsData = await GetAll();

        return allStatisticsData.First(x => x.TimeSlotId == timeSlotId);
    }

    public async Task<List<Domain.Entities.StatisticsData>> GetAll()
    {
        var allEvents = await statisticsDataEventStore.GetAllEventsAsync();

        var groupedEvents = allEvents
            .GroupBy(e => e.EntityId)
            .OrderBy(e => e.Key)
            .ToList();

        return groupedEvents
            .Select(group => Domain.Entities.StatisticsData.Rehydrate(group.ToList()))
            .ToList();
    }
}