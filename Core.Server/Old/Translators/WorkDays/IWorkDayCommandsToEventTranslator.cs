using Application.Contract.CQRS.Commands.Entities.WorkDays;
using Domain.Events.WorkDays;

namespace Application.Translators.WorkDays;

public interface IWorkDayCommandsToEventTranslator
{
    WorkDayEvent ToEvent(CreateWorkDayCommand createWorkDayCommand);
}