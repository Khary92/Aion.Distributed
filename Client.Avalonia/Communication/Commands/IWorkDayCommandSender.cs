using System.Threading.Tasks;
using Proto.Command.WorkDays;

namespace Client.Avalonia.Communication.Sender;

public interface IWorkDayCommandSender
{
    Task<bool> Send(CreateWorkDayCommand command);
}