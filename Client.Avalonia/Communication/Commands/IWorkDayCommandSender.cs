using System.Threading.Tasks;
using Proto.Command.WorkDays;

namespace Client.Avalonia.Communication.Commands;

public interface IWorkDayCommandSender
{
    Task<bool> Send(CreateWorkDayCommand command);
}