
namespace Contract.DTO.NonPersistent;

public class AnalysisByTag
{
    public List<TimeSlotDto> TimeSlots { get; init; } = [];
    public List<StatisticsDataDto> StatisticsData { get; init; } = [];
}