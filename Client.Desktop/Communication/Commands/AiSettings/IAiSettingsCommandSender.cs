using System.Threading.Tasks;
using Proto.Command.AiSettings;

namespace Client.Desktop.Communication.Commands.AiSettings;

public interface IAiSettingsCommandSender
{
    Task<bool> Send(ChangeLanguageModelCommandProto command);
    Task<bool> Send(ChangePromptCommandProto command);
    Task<bool> Send(CreateAiSettingsCommandProto command);
}