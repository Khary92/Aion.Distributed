using System.Collections.Generic;

namespace Client.Desktop.DataModels.Decorators.Entities;

public class AnalysisByTicket
{
    public string TicketName { get; init; } = string.Empty;
    public List<TimeSlotClientModel> TimeSlots { get; init; } = [];
    public List<StatisticsDataClientModel> StatisticData { get; init; } = [];
}