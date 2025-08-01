using System;

namespace Client.Desktop.Communication.Requests.Analysis.Records;

public record ClientGetSprintAnalysisById(Guid SprintId, Guid TraceId);