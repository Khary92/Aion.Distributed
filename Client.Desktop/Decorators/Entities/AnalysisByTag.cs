using System.Collections.Generic;
using Client.Desktop.DTO;

namespace Client.Desktop.Decorators.Entities;

public class AnalysisByTag
{
    public List<TimeSlotDto> TimeSlots { get; init; } = [];
    public List<StatisticsDataDto> StatisticsData { get; init; } = [];
}