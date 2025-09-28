using Client.Desktop.Communication.Commands.Client.Records;
using Google.Protobuf.WellKnownTypes;
using Proto.Command.Client;
using Proto.DTO.TraceData;

namespace Client.Desktop.Communication.Commands.Client;

public static class UseCaseExtensions
{
    public static CreateTrackingControlCommandProto ToProto(this ClientCreateTrackingControlCommand command)
    {
        return new CreateTrackingControlCommandProto
        {
            TicketId = command.TicketId.ToString(),
            Date = Timestamp.FromDateTimeOffset(command.Date),
            TraceData = new TraceDataProto
            {
                TraceId = command.TraceId.ToString()
            }
        };
    }
}