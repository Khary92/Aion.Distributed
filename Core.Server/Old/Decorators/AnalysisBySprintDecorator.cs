using Application.Contract.DTO.NonPersistent;

namespace Application.Decorators;

public class AnalysisBySprintDecorator(AnalysisBySprint analysisBySprint)
{
    private int _timeSpentNeutral;
    private int _timeSpentProductive;
    private int _timeSpentUnproductive;

    private bool _wasInitialized;

    public string SprintName => analysisBySprint.SprintName;
    public Dictionary<string, int> ProductiveTags => analysisBySprint.ProductiveTags;
    public Dictionary<string, int> NeutralTags => analysisBySprint.NeutralTags;
    public Dictionary<string, int> UnproductiveTags => analysisBySprint.UnproductiveTags;

    private int TotalTimeSpent { get; set; }

    private void InitializeTimesData()
    {
        if (_wasInitialized) return;

        foreach (var timeSlot in analysisBySprint.TimeSlots)
        {
            var statisticsData = analysisBySprint.StatisticsData.First(t => t.TimeSlotId == timeSlot.TimeSlotId);

            TotalTimeSpent += timeSlot.GetDurationInMinutes();

            if (statisticsData.IsProductive)
            {
                _timeSpentProductive += timeSlot.GetDurationInMinutes();
                continue;
            }

            if (statisticsData.IsNeutral)
            {
                _timeSpentNeutral += timeSlot.GetDurationInMinutes();
                continue;
            }

            if (statisticsData.IsUnproductive)
            {
                _timeSpentUnproductive += timeSlot.GetDurationInMinutes();
            }
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

    public Dictionary<string, int> GetMinutesSpentByTicket()
    {
        var minutesSpent = analysisBySprint.Tickets
            .Select(t => t.Name)
            .Distinct()
            .ToDictionary(name => name, _ => 0);

        var mappedDurations = analysisBySprint.TimeSlots
            .GroupBy(ts => ts.SelectedTicketId)
            .ToDictionary(
                g => analysisBySprint.Tickets.First(t => t.TicketId == g.Key).Name,
                g => g.Sum(ts => ts.GetDurationInMinutes())
            );

        foreach (var entry in mappedDurations) minutesSpent[entry.Key] = entry.Value;

        return minutesSpent;
    }
}