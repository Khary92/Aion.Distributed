using Service.Server.CQRS.Commands.Entities.WorkDays;

namespace Service.Server.Old.Services.Entities.WorkDays;

public interface IWorkDayCommandsService
{
    Task Create(CreateWorkDayCommand createWorkDayCommand);
}