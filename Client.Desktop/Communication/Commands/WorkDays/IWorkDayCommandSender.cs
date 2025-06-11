using System.Threading.Tasks;
using Proto.Command.WorkDays;

namespace Client.Desktop.Communication.Commands.WorkDays;

public interface IWorkDayCommandSender
{
    Task<bool> Send(CreateWorkDayCommand command);
}