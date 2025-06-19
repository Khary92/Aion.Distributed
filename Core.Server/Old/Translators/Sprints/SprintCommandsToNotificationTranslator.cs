using Service.Server.CQRS.Commands.Entities.Sprints;

namespace Service.Server.Old.Translators.Sprints;

public class SprintCommandsToNotificationTranslator : ISprintCommandsToNotificationTranslator
{
    public SprintCreatedNotification ToNotification(CreateSprintCommand createSprintCommand)
    {
        return new SprintCreatedNotification(createSprintCommand.SprintId, createSprintCommand.Name,
            createSprintCommand.StartTime, createSprintCommand.EndTime, createSprintCommand.IsActive,
            createSprintCommand.TicketIds);
    }

    public SprintDataUpdatedNotification ToNotification(UpdateSprintDataCommand updateSprintDataCommand)
    {
        return new SprintDataUpdatedNotification(updateSprintDataCommand.SprintId, updateSprintDataCommand.Name,
            updateSprintDataCommand.StartTime, updateSprintDataCommand.EndTime);
    }

    public SprintActiveStatusSetNotification ToNotification(SetSprintActiveStatusCommand setSprintActiveStatusCommand)
    {
        return new SprintActiveStatusSetNotification(setSprintActiveStatusCommand.SprintId,
            setSprintActiveStatusCommand.IsActive);
    }

    public TicketAddedToSprintNotification ToNotification(AddTicketToSprintCommand addTicketToSprintCommand)
    {
        return new TicketAddedToSprintNotification(addTicketToSprintCommand.SprintId,
            addTicketToSprintCommand.TicketId);
    }
}