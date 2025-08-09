using Client.Desktop.Communication.Commands.UseCases.Records;
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
            TraceData = new TraceDataProto
            {
                TraceId = command.TraceId.ToString()
            }
        };
    }
}