using System;

namespace Client.Desktop.Communication.Requests.Analysis.Records;

public record ClientGetTagAnalysisById(Guid TagId, Guid TraceId);