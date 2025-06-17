using Application.Contract.CQRS.Commands.Entities.WorkDays;
using Application.Contract.Notifications.Entities.WorkDays;

namespace Application.Translators.WorkDays;

public class WorkDayCommandsToNotificationTranslator : IWorkDayCommandsToNotificationTranslator
{
    public WorkDayCreatedNotification ToNotification(CreateWorkDayCommand createWorkDayCommand)
    {
        return new WorkDayCreatedNotification(createWorkDayCommand.WorkDayId, createWorkDayCommand.Date);
    }
}