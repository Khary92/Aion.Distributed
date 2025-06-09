using System.Threading.Tasks;
using Proto.Command.Settings;

namespace Client.Avalonia.Communication.Sender;

public interface ISettingsCommandSender
{
    Task<bool> Send(CreateSettingsCommand command);
    Task<bool> Send(UpdateSettingsCommand command);
}