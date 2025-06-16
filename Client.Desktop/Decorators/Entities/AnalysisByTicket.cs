
using System.Collections.Generic;
using Contract.DTO;

namespace Client.Desktop.Decorators.Entities;

public class AnalysisByTicket
{
    public string TicketName { get; init; } = string.Empty;
    public List<TimeSlotDto> TimeSlots { get; init; } = [];
    public List<StatisticsDataDto> StatisticData { get; init; } = [];
}