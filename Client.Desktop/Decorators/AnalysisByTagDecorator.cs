using System.Collections.Generic;
using System.Linq;
using Client.Desktop.Decorators.Entities;
using Proto.Shared;

namespace Client.Desktop.Decorators;

public class AnalysisByTagDecorator(AnalysisByTag analysisByTag)
{
    private int GetTotalDurationInSeconds()
    {
        return analysisByTag.TimeSlots.Sum(timeSlot => timeSlot.GetDurationInSeconds());
    }

    public Dictionary<Productivity, int> GetMinutesByProductivity()
    {
        var countMap = new Dictionary<Productivity, int>
        {
            { Productivity.Productive, 0 },
            { Productivity.Neutral, 0 },
            { Productivity.Unproductive, 0 }
        };

        foreach (var timeSlot in analysisByTag.TimeSlots)
        {
            var statisticsData = analysisByTag.StatisticsData.First(sd => sd.TimeSlotId == timeSlot.TimeSlotId);

            if (statisticsData.IsProductive)
            {
                countMap[Productivity.Productive] += timeSlot.GetDurationInMinutes();
                continue;
            }

            if (statisticsData.IsNeutral)
            {
                countMap[Productivity.Neutral] += timeSlot.GetDurationInMinutes();
                continue;
            }

            if (statisticsData.IsUnproductive) countMap[Productivity.Unproductive] += timeSlot.GetDurationInMinutes();
        }


        return countMap;
    }

    public Dictionary<Productivity, double> GetProductivityPercentageByTime()
    {
        var timeByProductivity = GetMinutesByProductivity();

        var percentageTimeByProductivity = new Dictionary<Productivity, double>
        {
            { Productivity.Productive, 0 },
            { Productivity.Neutral, 0 },
            { Productivity.Unproductive, 0 }
        };

        foreach (var key in percentageTimeByProductivity.Keys) timeByProductivity.TryAdd(key, 0);

        var totalDurationInSeconds = GetTotalDurationInSeconds();

        if (totalDurationInSeconds == 0) return percentageTimeByProductivity;

        percentageTimeByProductivity[Productivity.Productive] =
            (double)timeByProductivity[Productivity.Productive] / totalDurationInSeconds * 100;
        percentageTimeByProductivity[Productivity.Neutral] =
            (double)timeByProductivity[Productivity.Neutral] / totalDurationInSeconds * 100;
        percentageTimeByProductivity[Productivity.Unproductive] =
            (double)timeByProductivity[Productivity.Unproductive] / totalDurationInSeconds * 100;

        return percentageTimeByProductivity;
    }

    public Dictionary<Productivity, double> GetProductivityPercentages()
    {
        var countMap = new Dictionary<Productivity, double>
        {
            { Productivity.Productive, 0 },
            { Productivity.Neutral, 0 },

            { Productivity.Unproductive, 0 }
        };

        if (analysisByTag.StatisticsData.Count == 0) return countMap;

        foreach (var statisticsData in analysisByTag.StatisticsData)
        {
            if (statisticsData.IsProductive)
            {
                countMap[Productivity.Productive]++;
                continue;
            }

            if (statisticsData.IsNeutral)
            {
                countMap[Productivity.Neutral]++;
                continue;
            }

            if (statisticsData.IsUnproductive) countMap[Productivity.Unproductive]++;
        }

        var productivityPercentages = new Dictionary<Productivity, double>
        {
            {
                Productivity.Productive,
                (int)(countMap[Productivity.Productive] / analysisByTag.StatisticsData.Count * 100)
            },
            { Productivity.Neutral, (int)(countMap[Productivity.Neutral] / analysisByTag.StatisticsData.Count * 100) },
            {
                Productivity.Unproductive,
                (int)(countMap[Productivity.Unproductive] / analysisByTag.StatisticsData.Count * 100)
            }
        };

        return productivityPercentages;
    }
}