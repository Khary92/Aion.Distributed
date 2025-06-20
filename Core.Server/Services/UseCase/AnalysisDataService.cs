using Domain.Entities;
using Service.Server.Communication.Mapper;
using Service.Server.Old.Services.Entities.StatisticsData;
using Service.Server.Old.Services.Entities.Tags;
using Service.Server.Old.Services.Entities.Tickets;
using Service.Server.Old.Services.Entities.TimeSlots;
using Service.Server.Old.Services.Entities.WorkDays;

namespace Service.Server.Old.Services.UseCase;

public class AnalysisDataService(
    ITimeSlotRequestsService timeSlotRequestsService,
    IStatisticsDataRequestsService statisticsDataRequestsService,
    ITagRequestsService tagRequestsService,
    IWorkDayRequestsService workDayRequestsService,
    ITicketRequestsService ticketRequestsService,
    IDtoMapper<StatisticsDataDto, StatisticsData> statisticsDataMapper,
    IDtoMapper<TicketDto, Ticket> ticketMapper,
    IDtoMapper<TimeSlotDto, TimeSlot> timeSlotMapper) : IAnalysisDataService
{
    public async Task<AnalysisByTagDecorator> GetAnalysisByTag(TagDto tagDto)
    {
        var statisticsData = await statisticsDataRequestsService.GetAll();
        statisticsData = statisticsData.Where(sd => sd.TagIds.Contains(tagDto.TagId)).ToList();

        var timeSlots = new List<TimeSlotDto>();
        foreach (var data in statisticsData) timeSlots.Add(await timeSlotRequestsService.GetById(data.TimeSlotId));

        var analysisByTag = new AnalysisByTag
        {
            TimeSlots = timeSlots.Select(timeSlotMapper.ToDomain).ToList(),
            StatisticsData = statisticsData.Select(statisticsDataMapper.ToDomain).ToList()
        };

        return new AnalysisByTagDecorator(analysisByTag);
    }

    public async Task<AnalysisByTicketDecorator> GetAnalysisByTicket(TicketDto ticketDto)
    {
        var timeSlots = (await timeSlotRequestsService.GetAll())
            .Where(ts => ts.SelectedTicketId == ticketDto.TicketId).ToList();

        var statisticsData = new List<StatisticsDataDto>();
        foreach (var timeSlot in timeSlots)
            statisticsData.Add(
                await statisticsDataRequestsService.GetStatisticsDataByTimeSlotId(timeSlot.TimeSlotId));

        var analysisByTicket = new AnalysisByTicket
        {
            TicketName = ticketDto.Name,
            TimeSlots = timeSlots.Select(timeSlotMapper.ToDomain).ToList(),
            StatisticData = statisticsData.Select(statisticsDataMapper.ToDomain).ToList()
        };

        return new AnalysisByTicketDecorator(analysisByTicket, tagRequestsService);
    }

    public async Task<AnalysisBySprintDecorator> GetAnalysisBySprint(SprintDto sprintDto)
    {
        var domainWorkDays = await GetWorkDays(sprintDto.StartTime.Date, sprintDto.EndTime);
        var timeSlots = await GetTimeSlots(domainWorkDays);
        var statisticsData = await GetStatisticsData(timeSlots);
        var tagDtos = await tagRequestsService.GetAll();

        var (productiveTags, neutralTags, unproductiveTags) = GetTagCounts(statisticsData, tagDtos);

        var analysisBySprint = new AnalysisBySprint
        {
            SprintName = sprintDto.Name,
            TimeSlots = timeSlots.Select(timeSlotMapper.ToDomain).ToList(),
            StatisticsData = statisticsData.Select(statisticsDataMapper.ToDomain).ToList(),
            Tickets = (await ticketRequestsService.GetTicketsBySprintId(sprintDto.SprintId))
                .Select(ticketMapper.ToDomain).ToList(),
            ProductiveTags = productiveTags,
            NeutralTags = neutralTags,
            UnproductiveTags = unproductiveTags
        };

        return new AnalysisBySprintDecorator(analysisBySprint);
    }
    
    private async Task<List<WorkDayDto>> GetWorkDays(DateTimeOffset startDate, DateTimeOffset endDate)
    {
        return await workDayRequestsService.GetWorkDaysInDateRange(startDate, endDate);
    }

    private async Task<List<TimeSlotDto>> GetTimeSlots(List<WorkDayDto> domainWorkDays)
    {
        var timeSlots = new List<TimeSlotDto>();
        foreach (var domainWorkDay in domainWorkDays)
        {
            var slots = await timeSlotRequestsService.GetTimeSlotsForWorkDayId(domainWorkDay.WorkDayId);
            timeSlots.AddRange(slots);
        }

        return timeSlots;
    }

    private async Task<List<StatisticsDataDto>> GetStatisticsData(List<TimeSlotDto> timeSlots)
    {
        var statisticsData = new List<StatisticsDataDto>();
        foreach (var timeSlot in timeSlots)
        {
            var data = await statisticsDataRequestsService.GetStatisticsDataByTimeSlotId(timeSlot.TimeSlotId);
            statisticsData.Add(data);
        }

        return statisticsData;
    }

    private static (Dictionary<string, int> productiveTags, Dictionary<string, int> neutralTags, Dictionary<string, int>
        unproductiveTags)
        GetTagCounts(List<StatisticsDataDto> statisticsData, List<TagDto> tagDtos)
    {
        var productiveTags = new Dictionary<string, int>();
        var neutralTags = new Dictionary<string, int>();
        var unproductiveTags = new Dictionary<string, int>();

        foreach (var data in statisticsData) ProcessTags(data, tagDtos, productiveTags, neutralTags, unproductiveTags);

        return (productiveTags, neutralTags, unproductiveTags);
    }

    private static void ProcessTags(StatisticsDataDto data, List<TagDto> tagDtos,
        Dictionary<string, int> productiveTags, Dictionary<string, int> neutralTags,
        Dictionary<string, int> unproductiveTags)
    {
        if (data.IsProductive) AddTagsToDictionary(data.TagIds, tagDtos, productiveTags);

        if (data.IsNeutral) AddTagsToDictionary(data.TagIds, tagDtos, neutralTags);

        if (data.IsUnproductive) AddTagsToDictionary(data.TagIds, tagDtos, unproductiveTags);
    }

    private static void AddTagsToDictionary(IEnumerable<Guid> tagIds, List<TagDto> tagDtos,
        Dictionary<string, int> tagDictionary)
    {
        foreach (var tag in tagIds.Select(tagId => tagDtos.First(t => t.TagId == tagId)))
            if (!tagDictionary.TryAdd(tag.Name, 1))
                tagDictionary[tag.Name]++;
    }
}