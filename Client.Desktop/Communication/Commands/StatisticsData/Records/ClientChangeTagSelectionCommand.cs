using System;
using System.Collections.Generic;

namespace Client.Desktop.Communication.Commands.StatisticsData.Records;

public record ClientChangeTagSelectionCommand(
    Guid StatisticsDataId,
    List<Guid> SelectedTagIds,
    Guid TraceId);