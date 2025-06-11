using System.Threading.Tasks;
using Proto.Command.AiSettings;

namespace Client.Desktop.Communication.Commands.AiSettings;

public interface IAiSettingsCommandSender
{
    Task<bool> Send(ChangeLanguageModelCommand command);
    Task<bool> Send(ChangePromptCommand command);
    Task<bool> Send(CreateAiSettingsCommand command);
}