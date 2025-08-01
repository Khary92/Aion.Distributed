using System;

namespace Client.Desktop.Communication.Requests.Replays.Records;

public record ClientGetTicketReplaysByIdRequest(Guid TicketId, Guid TraceId);