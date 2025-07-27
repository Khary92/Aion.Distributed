using System;
using System.Collections.Generic;

namespace Client.Desktop.Communication.Commands.StatisticsData.Records;

public record ClientCreateStatisticsDataCommand(
    Guid StatisticsDataId,
    bool IsProductive,
    bool IsNeutral,
    bool IsUnproductive,
    List<Guid> TagIds,
    Guid TimeSlotId);