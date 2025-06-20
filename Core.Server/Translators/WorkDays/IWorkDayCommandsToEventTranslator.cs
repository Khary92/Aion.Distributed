using Core.Server.Communication.CQRS.Commands.Entities.WorkDays;
using Domain.Events.WorkDays;

namespace Core.Server.Translators.WorkDays;

public interface IWorkDayCommandsToEventTranslator
{
    WorkDayEvent ToEvent(CreateWorkDayCommand createWorkDayCommand);
}