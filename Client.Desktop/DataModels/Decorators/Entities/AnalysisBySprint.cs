using System.Collections.Generic;

namespace Client.Desktop.DataModels.Decorators.Entities;

public class AnalysisBySprint
{
    public string SprintName { get; init; } = string.Empty;
    public List<TimeSlotClientModel> TimeSlots { get; init; } = [];
    public List<StatisticsDataClientModel> StatisticsData { get; init; } = [];
    public List<TicketClientModel> Tickets { get; init; } = [];
    public Dictionary<string, int> ProductiveTags { get; init; } = [];
    public Dictionary<string, int> NeutralTags { get; init; } = [];
    public Dictionary<string, int> UnproductiveTags { get; init; } = [];
}