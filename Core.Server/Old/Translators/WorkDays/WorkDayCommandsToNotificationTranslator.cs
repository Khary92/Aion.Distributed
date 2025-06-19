using Service.Server.CQRS.Commands.Entities.WorkDays;

namespace Service.Server.Old.Translators.WorkDays;

public class WorkDayCommandsToNotificationTranslator : IWorkDayCommandsToNotificationTranslator
{
    public WorkDayCreatedNotification ToNotification(CreateWorkDayCommand createWorkDayCommand)
    {
        return new WorkDayCreatedNotification(createWorkDayCommand.WorkDayId, createWorkDayCommand.Date);
    }
}