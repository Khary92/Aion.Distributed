using Application.Contract.CQRS.Commands.Entities.Sprints;
using Application.Contract.Notifications.Entities.Sprints;

namespace Application.Translators.Sprints;

public interface ISprintCommandsToNotificationTranslator
{
    SprintCreatedNotification ToNotification(CreateSprintCommand createSprintCommand);
    SprintDataUpdatedNotification ToNotification(UpdateSprintDataCommand updateSprintDataCommand);
    SprintActiveStatusSetNotification ToNotification(SetSprintActiveStatusCommand setSprintActiveStatusCommand);
    TicketAddedToSprintNotification ToNotification(AddTicketToSprintCommand addTicketToSprintCommand);
}