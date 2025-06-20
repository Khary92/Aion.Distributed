using Domain.Events.WorkDays;
using Service.Server.Communication.CQRS.Commands.Entities.WorkDays;

namespace Service.Server.Translators.WorkDays;

public interface IWorkDayCommandsToEventTranslator
{
    WorkDayEvent ToEvent(CreateWorkDayCommand createWorkDayCommand);
}