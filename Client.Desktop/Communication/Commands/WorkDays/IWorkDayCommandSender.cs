using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.WorkDays.Records;
using Proto.Command.WorkDays;

namespace Client.Desktop.Communication.Commands.WorkDays;

public interface IWorkDayCommandSender
{
    Task<bool> Send(ClientCreateWorkDayCommand command);
}