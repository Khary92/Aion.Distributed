using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests.Analysis;
using Client.Desktop.Communication.Requests.Analysis.Records;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators;
using Client.Desktop.DataModels.Decorators.Entities;

namespace Client.Desktop.Communication.Mock.Requests;

public class MockAnalysisRequestSender(MockDataService mockDataService) : IAnalysisRequestSender
{
    public Task<AnalysisBySprintDecorator> Send(ClientGetSprintAnalysisById request)
    {
        var sprint = mockDataService.Sprints.First(s => s.SprintId == request.SprintId);
        var tickets = mockDataService.Tickets.Where(t => t.SprintIds.Contains(sprint.SprintId)).ToList();
        var timeSlots = mockDataService.TimeSlots.Where(ts => tickets.Any(t => t.TicketId == ts.SelectedTicketId))
            .ToList();
        var statisticsData = mockDataService.StatisticsData
            .Where(sd => timeSlots.Any(ts => ts.TimeSlotId == sd.TimeSlotId)).ToList();

        var result = GetTagCounts(statisticsData, mockDataService.Tags);

        var analysisBySprint = new AnalysisBySprint
        {
            SprintName = sprint.Name,
            TimeSlots = timeSlots,
            StatisticsData = statisticsData,
            Tickets = tickets,
            ProductiveTags = result.productiveTags,
            NeutralTags = result.neutralTags,
            UnproductiveTags = result.unproductiveTags
        };

        return Task.FromResult(new AnalysisBySprintDecorator(analysisBySprint));
    }

    public Task<AnalysisByTicketDecorator> Send(ClientGetTicketAnalysisById request)
    {
        var timeSlots = mockDataService.TimeSlots.Where(ts => ts.SelectedTicketId == request.TicketId).ToList();
        var statisticsData = mockDataService.StatisticsData
            .Where(sd => timeSlots.Any(ts => ts.TimeSlotId == sd.TimeSlotId)).ToList();
        var tags = mockDataService.Tags.Where(t => statisticsData.Any(sd => sd.TagIds.Contains(t.TagId))).ToList();

        var result = GetTagCounts(statisticsData, tags);

        var analysisByTicket = new AnalysisByTicket
        {
            TicketName = mockDataService.Tickets.First(t => t.TicketId == request.TicketId).Name,
            TimeSlots = timeSlots,
            StatisticData = statisticsData,
            ProductiveTags = result.productiveTags,
            NeutralTags = result.neutralTags,
            UnproductiveTags = result.unproductiveTags
        };

        return Task.FromResult(new AnalysisByTicketDecorator(analysisByTicket));
    }

    public Task<AnalysisByTagDecorator> Send(ClientGetTagAnalysisById request)
    {
        var staticsData = mockDataService.StatisticsData.Where(sd => sd.TagIds.Contains(request.TagId)).ToList();
        var timeslots = mockDataService.TimeSlots.Where(ts => staticsData.Any(sd => sd.TimeSlotId == ts.TimeSlotId))
            .ToList();

        var analysisByTag = new AnalysisByTag
        {
            StatisticsData = staticsData,
            TimeSlots = timeslots
        };

        return Task.FromResult(new AnalysisByTagDecorator(analysisByTag));
    }

    private static (Dictionary<string, int> productiveTags, Dictionary<string, int> neutralTags, Dictionary<string, int>
        unproductiveTags)
        GetTagCounts(List<StatisticsDataClientModel> statisticsData, List<TagClientModel> tagClientModels)
    {
        var productiveTags = new Dictionary<string, int>();
        var neutralTags = new Dictionary<string, int>();
        var unproductiveTags = new Dictionary<string, int>();

        foreach (var data in statisticsData)
            ProcessTags(data, tagClientModels, productiveTags, neutralTags, unproductiveTags);

        return (productiveTags, neutralTags, unproductiveTags);
    }

    private static void ProcessTags(StatisticsDataClientModel data, List<TagClientModel> tagClientModels,
        Dictionary<string, int> productiveTags, Dictionary<string, int> neutralTags,
        Dictionary<string, int> unproductiveTags)
    {
        if (data.IsProductive) AddTagsToDictionary(data.TagIds, tagClientModels, productiveTags);

        if (data.IsNeutral) AddTagsToDictionary(data.TagIds, tagClientModels, neutralTags);

        if (data.IsUnproductive) AddTagsToDictionary(data.TagIds, tagClientModels, unproductiveTags);
    }

    private static void AddTagsToDictionary(IEnumerable<Guid> tagIds, List<TagClientModel> tagClientModels,
        Dictionary<string, int> tagDictionary)
    {
        foreach (var tag in tagIds.Select(tagId => tagClientModels.First(t => t.TagId == tagId)))
            if (!tagDictionary.TryAdd(tag.Name, 1))
                tagDictionary[tag.Name]++;
    }
}