using Core.Server.Communication.Records.Commands.Entities.Tickets;

namespace Core.Server.Services.Entities.Tickets;

public interface ITicketCommandsService
{
    Task UpdateData(UpdateTicketDataCommand updateTicketDataCommand);
    Task UpdateDocumentation(UpdateTicketDocumentationCommand updateTicketDocumentationCommand);
    Task Create(CreateTicketCommand createTicketCommand);
}