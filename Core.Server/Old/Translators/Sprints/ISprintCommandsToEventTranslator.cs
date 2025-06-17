using Application.Contract.CQRS.Commands.Entities.Sprints;
using Domain.Events.Sprints;

namespace Application.Translators.Sprints;

public interface ISprintCommandsToEventTranslator
{
    SprintEvent ToEvent(CreateSprintCommand createSprintCommand);
    SprintEvent ToEvent(UpdateSprintDataCommand updateSprintDataCommand);
    SprintEvent ToEvent(SetSprintActiveStatusCommand setSprintActiveStatusCommand);
    SprintEvent ToEvent(AddTicketToSprintCommand addTicketToSprintCommand);
}