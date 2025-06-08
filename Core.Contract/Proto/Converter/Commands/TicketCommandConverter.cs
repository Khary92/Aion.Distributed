using Contract.CQRS.Commands.Entities.Tickets;
using Proto.Command.Tickets;

namespace Contract.Proto.Converter.Commands
{
    public static class TicketCommandConverter
    {
        public static CreateTicketProtoCommand ToProto(this CreateTicketCommand command)
            => new()
            {
                TicketId = command.TicketId.ToString(),
                Name = command.Name,
                BookingNumber = command.BookingNumber,
                SprintIds = { command.SprintIds.Select(id => id.ToString()) }
            };

        public static UpdateTicketDataProtoCommand ToProto(this UpdateTicketDataCommand command)
            => new()
            {
                TicketId = command.TicketId.ToString(),
                Name = command.Name,
                BookingNumber = command.BookingNumber,
                SprintIds = { command.SprintIds.Select(id => id.ToString()) }
            };

        public static UpdateTicketDocumentationProtoCommand ToProto(this UpdateTicketDocumentationCommand command)
            => new()
            {
                TicketId = command.TicketId.ToString(),
                Documentation = command.Documentation
            };
    }
}