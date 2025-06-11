using System.Threading.Tasks;
using Proto.Command.Settings;

namespace Client.Desktop.Communication.Commands.Settings;

public interface ISettingsCommandSender
{
    Task<bool> Send(CreateSettingsCommand command);
    Task<bool> Send(UpdateSettingsCommand command);
}