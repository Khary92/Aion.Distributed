using Client.Desktop.Communication.Commands.UseCases.Records;
using Google.Protobuf.WellKnownTypes;
using Proto.Command.UseCases;
using Proto.DTO.TraceData;

namespace Client.Desktop.Communication.Commands.UseCases;

public static class UseCaseExtensions
{
    public static CreateTrackingControlCommandProto ToProto(this ClientCreateTimeSlotControlCommand command)
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