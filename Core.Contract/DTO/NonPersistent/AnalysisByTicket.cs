namespace Contract.DTO.NonPersistent;

public class AnalysisByTicket
{
    public string TicketName { get; init; } = string.Empty;
    public List<TimeSlotDto> TimeSlots { get; init; } = [];
    public List<StatisticsDataDto> StatisticData { get; init; } = [];
}