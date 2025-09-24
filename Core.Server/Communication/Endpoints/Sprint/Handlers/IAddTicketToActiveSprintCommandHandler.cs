using Core.Server.Communication.Records.Commands.Entities.Sprints;

namespace Core.Server.Communication.Endpoints.Sprint.Handlers;

public interface IAddTicketToActiveSprintCommandHandler
{
    Task Handle(AddTicketToActiveSprintCommand command);
}