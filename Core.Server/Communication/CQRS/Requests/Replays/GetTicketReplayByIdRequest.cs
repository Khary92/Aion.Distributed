namespace Core.Server.Communication.CQRS.Requests.Replays;

public record GetTicketReplayByIdRequest(Guid TicketId);