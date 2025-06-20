using Domain.Events.Sprints;
using Service.Server.CQRS.Commands.Entities.Sprints;

namespace Service.Server.Old.Translators.Sprints;

public interface ISprintCommandsToEventTranslator
{
    SprintEvent ToEvent(CreateSprintCommand createSprintCommand);
    SprintEvent ToEvent(UpdateSprintDataCommand updateSprintDataCommand);
    SprintEvent ToEvent(SetSprintActiveStatusCommand setSprintActiveStatusCommand);
    SprintEvent ToEvent(AddTicketToSprintCommand addTicketToSprintCommand);
}