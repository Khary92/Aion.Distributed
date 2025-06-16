
using System.Collections.Generic;
using Contract.DTO;

namespace Client.Desktop.Decorators.Entities;

public class AnalysisByTag
{
    public List<TimeSlotDto> TimeSlots { get; init; } = [];
    public List<StatisticsDataDto> StatisticsData { get; init; } = [];
}