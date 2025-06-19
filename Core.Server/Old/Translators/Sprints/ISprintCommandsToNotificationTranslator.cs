using Service.Server.CQRS.Commands.Entities.Sprints;

namespace Service.Server.Old.Translators.Sprints;

public interface ISprintCommandsToNotificationTranslator
{
    SprintCreatedNotification ToNotification(CreateSprintCommand createSprintCommand);
    SprintDataUpdatedNotification ToNotification(UpdateSprintDataCommand updateSprintDataCommand);
    SprintActiveStatusSetNotification ToNotification(SetSprintActiveStatusCommand setSprintActiveStatusCommand);
    TicketAddedToSprintNotification ToNotification(AddTicketToSprintCommand addTicketToSprintCommand);
}