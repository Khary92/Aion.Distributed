using Core.Server.Communication.Records.Commands.Entities.Sprints;

namespace Core.Server.Communication.Endpoints.Sprint.Handlers;

public interface ISetSprintActiveStatusCommandHandler
{
    Task Handle(SetSprintActiveStatusCommand command);
}