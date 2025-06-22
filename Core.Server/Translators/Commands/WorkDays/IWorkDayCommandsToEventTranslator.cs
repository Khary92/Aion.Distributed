using Core.Server.Communication.Records.Commands.Entities.WorkDays;
using Domain.Events.WorkDays;

namespace Core.Server.Translators.Commands.WorkDays;

public interface IWorkDayCommandsToEventTranslator
{
    WorkDayEvent ToEvent(CreateWorkDayCommand createWorkDayCommand);
}