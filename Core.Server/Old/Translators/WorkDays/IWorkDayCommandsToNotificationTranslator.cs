using Application.Contract.CQRS.Commands.Entities.WorkDays;
using Application.Contract.Notifications.Entities.WorkDays;

namespace Application.Translators.WorkDays;

public interface IWorkDayCommandsToNotificationTranslator
{
    WorkDayCreatedNotification ToNotification(CreateWorkDayCommand createWorkDayCommand);
}