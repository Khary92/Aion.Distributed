using Core.Server.Communication.Records.Commands.Entities.Sprints;

namespace Core.Server.Services.Entities.Sprints;

public interface ISprintCommandsService
{
    Task Create(CreateSprintCommand command);
    Task UpdateSprintData(UpdateSprintDataCommand command);
    Task SetSprintActiveStatus(SetSprintActiveStatusCommand command);
    Task AddTicketToSprint(AddTicketToSprintCommand command);
}