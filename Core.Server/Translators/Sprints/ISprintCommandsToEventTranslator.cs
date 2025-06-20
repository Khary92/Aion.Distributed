using Core.Server.Communication.CQRS.Commands.Entities.Sprints;
using Domain.Events.Sprints;

namespace Core.Server.Translators.Sprints;

public interface ISprintCommandsToEventTranslator
{
    SprintEvent ToEvent(CreateSprintCommand createSprintCommand);
    SprintEvent ToEvent(UpdateSprintDataCommand updateSprintDataCommand);
    SprintEvent ToEvent(SetSprintActiveStatusCommand setSprintActiveStatusCommand);
    SprintEvent ToEvent(AddTicketToSprintCommand addTicketToSprintCommand);
}