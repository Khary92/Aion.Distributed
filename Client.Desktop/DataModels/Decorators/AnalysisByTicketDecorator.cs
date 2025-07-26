using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests;
using Client.Desktop.DataModels.Decorators.Entities;
using Client.Proto;
using Google.Protobuf.Collections;
using Proto.Requests.Tags;
using Service.Proto.Shared.Requests.Tags;

namespace Client.Desktop.DataModels.Decorators;

public class AnalysisByTicketDecorator(AnalysisByTicket analysisByTicket, ITagRequestSender requestSender)
{
    public readonly string TicketName = analysisByTicket.TicketName;
    private int _timeSpentNeutral;
    private int _timeSpentProductive;
    private int _timeSpentUnproductive;

    private bool _wasInitialized;
    public int TotalTimeSpent { get; private set; }

    private void InitializeTimesData()
    {
        if (_wasInitialized) return;

        foreach (var timeSlot in analysisByTicket.TimeSlots)
        {
            var statisticsData = analysisByTicket.StatisticData.First(t => t.TimeSlotId == timeSlot.TimeSlotId);

            TotalTimeSpent += timeSlot.GetDurationInMinutes();

            if (statisticsData.IsProductive)
            {
                _timeSpentProductive += timeSlot.GetDurationInMinutes();
                return;
            }

            if (statisticsData.IsNeutral)
            {
                _timeSpentNeutral += timeSlot.GetDurationInMinutes();
                return;
            }

            if (statisticsData.IsUnproductive) _timeSpentUnproductive += timeSlot.GetDurationInMinutes();
        }

        _wasInitialized = true;
    }

    public Dictionary<Productivity, int> GetTimeSpentInProductivity()
    {
        InitializeTimesData();

        return new Dictionary<Productivity, int>
        {
            { Productivity.Productive, _timeSpentProductive },
            { Productivity.Neutral, _timeSpentNeutral },
            { Productivity.Unproductive, _timeSpentUnproductive }
        };
    }

    public async Task<Dictionary<Productivity, List<Guid>>> GetMostRepresentedTagIdsByProductivity()
    {
        var tagIds = analysisByTicket.StatisticData
            .SelectMany(statisticsData => statisticsData.TagIds)
            .Distinct()
            .ToList();

        var tagIdsRepeated = new RepeatedField<string>();
        foreach (var tagId in tagIds) tagIdsRepeated.Add(tagId.ToString());

        var tagListProto = await requestSender.Send(new GetTagsByIdsRequestProto
        {
            TagIds = { tagIdsRepeated }
        });

        var usedTags = tagListProto.ToModelList();

        var countByTagProductive = usedTags.ToDictionary(tag => tag.TagId, _ => 0);
        var countByTagNeutral = usedTags.ToDictionary(tag => tag.TagId, _ => 0);
        var countByTagUnproductive = usedTags.ToDictionary(tag => tag.TagId, _ => 0);

        analysisByTicket.StatisticData
            .SelectMany(statisticData => statisticData.TagIds.Select(tagId => new { statisticData, tagId }))
            .ToList()
            .ForEach(x =>
            {
                if (x.statisticData.IsProductive)
                {
                    countByTagProductive[x.tagId]++;
                    return;
                }

                if (x.statisticData.IsNeutral)
                {
                    countByTagNeutral[x.tagId]++;
                    return;
                }

                if (x.statisticData.IsUnproductive) countByTagUnproductive[x.tagId]++;
            });

        var mostProductiveTagIds = countByTagProductive
            .OrderByDescending(vk => vk.Value)
            .Take(3)
            .Where(vk => vk.Value > 0)
            .Select(vk => vk.Key)
            .ToList();

        var mostNeutralTagIds = countByTagNeutral
            .OrderByDescending(vk => vk.Value)
            .Take(3)
            .Where(vk => vk.Value > 0)
            .Select(vk => vk.Key)
            .ToList();

        var mostUnproductiveTagIds = countByTagUnproductive
            .OrderByDescending(vk => vk.Value)
            .Take(3)
            .Where(vk => vk.Value > 0)
            .Select(vk => vk.Key)
            .ToList();

        Dictionary<Productivity, List<Guid>> mostRepresentedTag = new()
        {
            [Productivity.Productive] = mostProductiveTagIds,
            [Productivity.Neutral] = mostNeutralTagIds,
            [Productivity.Unproductive] = mostUnproductiveTagIds
        };

        return mostRepresentedTag;
    }

    public int CountTagByProductivity(Productivity productivity, Guid tagId)
    {
        return productivity switch
        {
            Productivity.Productive => analysisByTicket.StatisticData
                .Where(statisticsData => statisticsData.IsProductive)
                .Count(statisticsData => statisticsData.TagIds.Contains(tagId)),
            Productivity.Neutral => analysisByTicket.StatisticData.Where(statisticsData => statisticsData.IsNeutral)
                .Count(statisticsData => statisticsData.TagIds.Contains(tagId)),
            _ => analysisByTicket.StatisticData.Where(statisticsData => statisticsData.IsUnproductive)
                .Count(statisticsData => statisticsData.TagIds.Contains(tagId))
        };
    }
}