
namespace Contract.DTO.NonPersistent;

public class AnalysisByAll
{
    public List<TimeSlotDto> TimeSlots { get; init; } = [];
    public List<StatisticsDataDto> StatisticsData { get; init; } = [];
}