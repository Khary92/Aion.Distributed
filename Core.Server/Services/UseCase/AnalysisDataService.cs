using Core.Server.Communication.Services.StatisticsData;
using Core.Server.Communication.Services.Ticket;
using Core.Server.Communication.Services.TimeSlot;
using Core.Server.Services.Entities.StatisticsData;
using Core.Server.Services.Entities.Tags;
using Core.Server.Services.Entities.Tickets;
using Core.Server.Services.Entities.TimeSlots;
using Core.Server.Services.Entities.WorkDays;
using Domain.Entities;
using Google.Protobuf.Collections;
using Proto.DTO.AnalysisBySprint;
using Proto.DTO.AnalysisByTag;
using Proto.DTO.AnalysisByTicket;
using Proto.DTO.StatisticsData;
using Proto.DTO.Ticket;
using Proto.DTO.TimeSlots;

namespace Core.Server.Services.UseCase;

public class AnalysisDataService(
    ITimeSlotRequestsService timeSlotRequestsService,
    IStatisticsDataRequestsService statisticsDataRequestsService,
    ITagRequestsService tagRequestsService,
    IWorkDayRequestsService workDayRequestsService,
    ITicketRequestsService ticketRequestsService) : IAnalysisDataService
{
    public async Task<AnalysisByTagProto> GetAnalysisByTag(Tag tag)
    {
        var statisticsData = await statisticsDataRequestsService.GetAll();
        statisticsData = statisticsData.Where(sd => sd.TagIds.Contains(tag.TagId)).ToList();

        var timeSlots = new List<TimeSlot>();
        foreach (var data in statisticsData) timeSlots.Add(await timeSlotRequestsService.GetById(data.TimeSlotId));

        return new AnalysisByTagProto
        {
            TimeSlots = { ConvertToRepeatedField(timeSlots) },
            StatisticsData = { ConvertToRepeatedField(statisticsData) }
        };
    }

    public async Task<AnalysisByTicketProto> GetAnalysisByTicket(Ticket ticket)
    {
        var timeSlots = (await timeSlotRequestsService.GetAll())
            .Where(ts => ts.SelectedTicketId == ticket.TicketId).ToList();

        var statisticsData = new List<StatisticsData>();
        foreach (var timeSlot in timeSlots)
            statisticsData.Add(
                await statisticsDataRequestsService.GetStatisticsDataByTimeSlotId(timeSlot.TimeSlotId));

        return new AnalysisByTicketProto
        {
            TicketName = ticket.Name,
            TimeSlots = { ConvertToRepeatedField(timeSlots) },
            StatisticData = { ConvertToRepeatedField(statisticsData) }
        };
    }

    public async Task<AnalysisBySprintProto> GetAnalysisBySprint(Sprint sprint)
    {
        var workDays = await workDayRequestsService.GetWorkDaysInDateRange(sprint.StartDate.Date, sprint.EndDate.Date);
        var timeSlots = await GetTimeSlots(workDays);
        var statisticsData = await GetStatisticsData(timeSlots);
        var tags = await tagRequestsService.GetAll();
        var tickets = await ticketRequestsService.GetTicketsBySprintId(sprint.SprintId);

        var (productiveTags, neutralTags, unproductiveTags) = GetTagCounts(statisticsData, tags);

        return new AnalysisBySprintProto
        {
            SprintName = sprint.Name,
            TimeSlots = { ConvertToRepeatedField(timeSlots) },
            StatisticsData = { ConvertToRepeatedField(statisticsData) },
            Tickets = { ConvertToRepeatedField(tickets) },
            ProductiveTags = { new MapField<string, int> { productiveTags } },
            NeutralTags = { new MapField<string, int> { neutralTags } },
            UnproductiveTags = { new MapField<string, int> { unproductiveTags } }
        };
    }

    private static RepeatedField<TimeSlotProto> ConvertToRepeatedField(List<TimeSlot> timeSlots)
    {
        var repeatedTimeSlots = new RepeatedField<TimeSlotProto>();
        foreach (var timeSlot in timeSlots) repeatedTimeSlots.Add(timeSlot.ToProto());

        return repeatedTimeSlots;
    }

    private static RepeatedField<StatisticsDataProto> ConvertToRepeatedField(List<StatisticsData> statisticsData)
    {
        var repeatedStatisticsData = new RepeatedField<StatisticsDataProto>();
        foreach (var statistic in statisticsData) repeatedStatisticsData.Add(statistic.ToProto());

        return repeatedStatisticsData;
    }

    private static RepeatedField<TicketProto> ConvertToRepeatedField(List<Ticket> tickets)
    {
        var repeatedTickets = new RepeatedField<TicketProto>();
        foreach (var ticket in tickets) repeatedTickets.Add(ticket.ToProto());

        return repeatedTickets;
    }

    private async Task<List<TimeSlot>> GetTimeSlots(List<WorkDay> domainWorkDays)
    {
        var timeSlots = new List<TimeSlot>();
        foreach (var domainWorkDay in domainWorkDays)
        {
            var slots = await timeSlotRequestsService.GetTimeSlotsForWorkDayId(domainWorkDay.WorkDayId);
            timeSlots.AddRange(slots);
        }

        return timeSlots;
    }

    private async Task<List<StatisticsData>> GetStatisticsData(List<TimeSlot> timeSlots)
    {
        var statisticsData = new List<StatisticsData>();
        foreach (var timeSlot in timeSlots)
        {
            var data = await statisticsDataRequestsService.GetStatisticsDataByTimeSlotId(timeSlot.TimeSlotId);
            statisticsData.Add(data);
        }

        return statisticsData;
    }

    private static (Dictionary<string, int> productiveTags, Dictionary<string, int> neutralTags, Dictionary<string, int>
        unproductiveTags)
        GetTagCounts(List<StatisticsData> statisticsData, List<Tag> tagDtos)
    {
        var productiveTags = new Dictionary<string, int>();
        var neutralTags = new Dictionary<string, int>();
        var unproductiveTags = new Dictionary<string, int>();

        foreach (var data in statisticsData) ProcessTags(data, tagDtos, productiveTags, neutralTags, unproductiveTags);

        return (productiveTags, neutralTags, unproductiveTags);
    }

    private static void ProcessTags(StatisticsData data, List<Tag> tagDtos,
        Dictionary<string, int> productiveTags, Dictionary<string, int> neutralTags,
        Dictionary<string, int> unproductiveTags)
    {
        if (data.IsProductive) AddTagsToDictionary(data.TagIds, tagDtos, productiveTags);

        if (data.IsNeutral) AddTagsToDictionary(data.TagIds, tagDtos, neutralTags);

        if (data.IsUnproductive) AddTagsToDictionary(data.TagIds, tagDtos, unproductiveTags);
    }

    private static void AddTagsToDictionary(IEnumerable<Guid> tagIds, List<Tag> tagDtos,
        Dictionary<string, int> tagDictionary)
    {
        foreach (var tag in tagIds.Select(tagId => tagDtos.First(t => t.TagId == tagId)))
            if (!tagDictionary.TryAdd(tag.Name, 1))
                tagDictionary[tag.Name]++;
    }
}