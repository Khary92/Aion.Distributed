using System.Collections.Generic;

namespace Client.Desktop.DataModels.Decorators.Entities;

public class AnalysisByTicket
{
    public string TicketName { get; init; } = string.Empty;
    public List<TimeSlotClientModel> TimeSlots { get; init; } = [];
    public List<StatisticsDataClientModel> StatisticData { get; init; } = [];
    public Dictionary<string, int> ProductiveTags { get; init; } = [];
    public Dictionary<string, int> NeutralTags { get; init; } = [];
    public Dictionary<string, int> UnproductiveTags { get; init; } = [];
}