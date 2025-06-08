using Contract.CQRS.Commands.UseCase;
using Proto.Command.UseCase;

namespace Contract.Proto.Converter.Commands
{
    public static class UseCaseCommandConverter
    {
        public static CreateTimeSlotControlProtoCommand ToProto(this CreateTimeSlotControlCommand command) => new()
        {
            TicketId = command.TicketId.ToString(),
            ViewId = command.ViewId.ToString()
        };

        public static LoadTimeSlotControlProtoCommand ToProto(this LoadTimeSlotControlCommand command) => new()
        {
            TimeSlotId = command.TimeSlotId.ToString(),
            ViewId = command.ViewId.ToString()
        };
    }
}