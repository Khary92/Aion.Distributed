using Service.Server.CQRS.Commands.Entities.WorkDays;

namespace Service.Server.Old.Translators.WorkDays;

public interface IWorkDayCommandsToNotificationTranslator
{
    WorkDayCreatedNotification ToNotification(CreateWorkDayCommand createWorkDayCommand);
}