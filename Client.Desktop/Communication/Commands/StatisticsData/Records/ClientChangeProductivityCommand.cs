using System;

namespace Client.Desktop.Communication.Commands.StatisticsData.Records;

public record ClientChangeProductivityCommand(
    Guid StatisticsDataId,
    bool IsProductive,
    bool IsNeutral,
    bool IsUnproductive);