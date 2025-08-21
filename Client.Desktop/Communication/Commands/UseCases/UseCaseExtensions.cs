using Client.Desktop.Communication.Commands.UseCases.Records;
using Google.Protobuf.WellKnownTypes;
using Proto.Command.UseCases;
using Proto.DTO.TraceData;

namespace Client.Desktop.Communication.Commands.UseCases;

public static class UseCaseExtensions
{
    public static CreateTimeSlotControlCommandProto ToProto(this ClientCreateTimeSlotControlCommand command)
    {
        return new CreateTimeSlotControlCommandProto
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