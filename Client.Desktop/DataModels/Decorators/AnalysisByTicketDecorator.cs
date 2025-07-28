using System;
using System.Collections.Generic;
using System.Linq;
using Client.Desktop.DataModels.Decorators.Entities;
using Client.Proto;

namespace Client.Desktop.DataModels.Decorators;

public class AnalysisByTicketDecorator(AnalysisByTicket analysisByTicket)
{
    public readonly string TicketName = analysisByTicket.TicketName;
    private int _timeSpentNeutral;
    private int _timeSpentProductive;
    private int _timeSpentUnproductive;

    private bool _wasInitialized;
    public int TotalTimeSpent { get; private set; }

    public Dictionary<string, int> ProductiveTags => analysisByTicket.ProductiveTags;
    public Dictionary<string, int> NeutralTags => analysisByTicket.NeutralTags;
    public Dictionary<string, int> UnproductiveTags => analysisByTicket.UnproductiveTags;

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