using System.Collections.Generic;

namespace Client.Desktop.DataModels.Decorators.Entities;

public class AnalysisByTag
{
    public List<TimeSlotClientModel> TimeSlots { get; init; } = [];
    public List<StatisticsDataClientModel> StatisticsData { get; init; } = [];
}