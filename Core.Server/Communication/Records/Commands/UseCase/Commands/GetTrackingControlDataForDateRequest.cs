namespace Core.Server.Communication.Records.Commands.UseCase.Commands;

public record GetTrackingControlDataForDateRequest(DateTimeOffset Date, Guid TraceId);