using Client.Desktop.Communication.Commands.UseCases.Records;
using Proto.Command.UseCases;

namespace Client.Desktop.Communication.Commands.UseCases;

public static class UseCaseExtensions
{
    public static CreateTimeSlotControlCommandProto ToProto(this ClientCreateTimeSlotControlCommand command)
    {
        return new CreateTimeSlotControlCommandProto
        {
            TicketId = command.TicketId.ToString(),
            TraceData = new()
            {
                TraceId = command.TraceId.ToString()
            }
        };
    }
}