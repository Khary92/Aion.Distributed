using Client.Desktop.Communication.Commands.UseCases.Records;
using Proto.Command.UseCases;

namespace Client.Desktop.Communication.Commands.UseCases;

public static class UseCaseExtensions
{
    public static CreateTimeSlotControlCommandProto ToProto(this ClientCreateTimeSlotControlCommand command) => new()
    {
        TicketId = command.TicketId.ToString(),
    };
}