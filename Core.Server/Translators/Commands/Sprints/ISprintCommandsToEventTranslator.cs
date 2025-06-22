using Core.Server.Communication.Records.Commands.Entities.Sprints;
using Domain.Events.Sprints;

namespace Core.Server.Translators.Commands.Sprints;

public interface ISprintCommandsToEventTranslator
{
    SprintEvent ToEvent(CreateSprintCommand createSprintCommand);
    SprintEvent ToEvent(UpdateSprintDataCommand updateSprintDataCommand);
    SprintEvent ToEvent(SetSprintActiveStatusCommand setSprintActiveStatusCommand);
    SprintEvent ToEvent(AddTicketToSprintCommand addTicketToSprintCommand);
}