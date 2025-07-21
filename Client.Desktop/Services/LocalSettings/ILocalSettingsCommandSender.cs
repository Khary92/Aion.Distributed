using System.Threading.Tasks;
using Client.Desktop.Services.LocalSettings.Commands;

namespace Client.Desktop.Services.LocalSettings;

public interface ILocalSettingsCommandSender
{
    void Send(SetExportPathCommand command);
}