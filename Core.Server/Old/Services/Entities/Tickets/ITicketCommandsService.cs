using Application.Contract.CQRS.Commands.Entities.Tickets;

namespace Application.Services.Entities.Tickets;

public interface ITicketCommandsService
{
    Task UpdateData(UpdateTicketDataCommand updateTicketDataCommand);
    Task UpdateDocumentation(UpdateTicketDocumentationCommand updateTicketDocumentationCommand);
    Task Create(CreateTicketCommand createTicketCommand);
}