using Domain.Events.WorkDays;
using Service.Server.CQRS.Commands.Entities.WorkDays;

namespace Service.Server.Old.Translators.WorkDays;

public interface IWorkDayCommandsToEventTranslator
{
    WorkDayEvent ToEvent(CreateWorkDayCommand createWorkDayCommand);
}