using Core.Server.Communication.CQRS.Commands.Entities.Sprints;

namespace Core.Server.Services.Entities.Sprints;

public interface ISprintCommandsService
{
    Task Create(CreateSprintCommand command);
    Task UpdateSprintData(UpdateSprintDataCommand updateSprintDataCommand);
    Task SetSprintActiveStatus(SetSprintActiveStatusCommand setSprintActiveStatusCommand);
    Task AddTicketToSprint(AddTicketToSprintCommand addTicketToSprintCommand);
}