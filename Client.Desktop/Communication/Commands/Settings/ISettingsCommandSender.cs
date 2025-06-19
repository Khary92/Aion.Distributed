using System.Threading.Tasks;
using Proto.Command.Settings;

namespace Client.Desktop.Communication.Commands.Settings;

public interface ISettingsCommandSender
{
    Task<bool> Send(CreateSettingsCommandProto command);
    Task<bool> Send(ChangeExportPathCommandProto command);
    Task<bool> Send(ChangeAutomaticTicketAddingToSprintCommandProto command);
}