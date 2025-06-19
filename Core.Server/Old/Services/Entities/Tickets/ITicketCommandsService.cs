using Service.Server.CQRS.Commands.Entities.Tickets;

namespace Service.Server.Old.Services.Entities.Tickets;

public interface ITicketCommandsService
{
    Task UpdateData(UpdateTicketDataCommand updateTicketDataCommand);
    Task UpdateDocumentation(UpdateTicketDocumentationCommand updateTicketDocumentationCommand);
    Task Create(CreateTicketCommand createTicketCommand);
}