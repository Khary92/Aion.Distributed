using Domain.Events.Sprints;
using Service.Server.Communication.CQRS.Commands.Entities.Sprints;

namespace Service.Server.Translators.Sprints;

public interface ISprintCommandsToEventTranslator
{
    SprintEvent ToEvent(CreateSprintCommand createSprintCommand);
    SprintEvent ToEvent(UpdateSprintDataCommand updateSprintDataCommand);
    SprintEvent ToEvent(SetSprintActiveStatusCommand setSprintActiveStatusCommand);
    SprintEvent ToEvent(AddTicketToSprintCommand addTicketToSprintCommand);
}