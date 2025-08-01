using Core.Server.Communication.Records.Commands.Entities.Tickets;

namespace Core.Server.Services.Entities.Tickets;

public interface ITicketCommandsService
{
    Task UpdateData(UpdateTicketDataCommand command);
    Task UpdateDocumentation(UpdateTicketDocumentationCommand command);
    Task Create(CreateTicketCommand command);
}