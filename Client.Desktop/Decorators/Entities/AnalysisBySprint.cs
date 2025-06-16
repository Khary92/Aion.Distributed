using System.Collections.Generic;
using Client.Desktop.DTO;

namespace Client.Desktop.Decorators.Entities;

public class AnalysisBySprint
{
    public string SprintName { get; init; } = string.Empty;
    public List<TimeSlotDto> TimeSlots { get; init; } = [];
    public List<StatisticsDataDto> StatisticsData { get; init; } = [];
    public List<TicketDto> Tickets { get; init; } = [];
    public Dictionary<string, int> ProductiveTags { get; init; } = [];
    public Dictionary<string, int> NeutralTags { get; init; } = [];
    public Dictionary<string, int> UnproductiveTags { get; init; } = [];
}