using System;

namespace Client.Desktop.Communication.Requests.Analysis.Records;

public record ClientGetTicketAnalysisById(Guid TicketId, Guid TraceId);