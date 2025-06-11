using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Requests;
using Contract.DTO;
using Proto.Command.AiSettings;
using ReactiveUI;

namespace Client.Desktop.Models.Settings;

public class AiSettingsModel(
    ICommandSender commandSender,
    IRequestSender requestSender) : ReactiveObject
{
    private string _previousLanguageModelPath = null!;

    private string _previousPrompt = null!;

    public AiSettingsDto? AiSettings { get; private set; }

    public async Task Initialize()
    {
        //TODO this is wrong. Obviously
        AiSettings = await requestSender.Get(Guid.NewGuid().ToString());
        _previousPrompt = AiSettings!.Prompt;
        _previousLanguageModelPath = AiSettings.LanguageModelPath;
    }

    public async Task ChangePrompt()
    {
        if (_previousPrompt == AiSettings!.Prompt)
        {
            //tracer.AiSettings.ChangePrompt.PropertyNotChanged(GetType(), AiSettings.AiSettingsId,
           //     ("prompt", AiSettings!.Prompt));
            return;
        }

        var changePromptCommand = new ChangePromptCommand
        {
            AiSettingsId = AiSettings.AiSettingsId.ToString(),
            Prompt = AiSettings.Prompt
        };

        await commandSender.Send(changePromptCommand);

        //tracer.AiSettings.ChangePrompt.CommandSent(GetType(), AiSettings.AiSettingsId, changePromptCommand);

        _previousPrompt = AiSettings.Prompt;
    }

    public async Task ChangeLanguageModelPath()
    {
        if (_previousLanguageModelPath == AiSettings!.LanguageModelPath)
        {
            //tracer.AiSettings.ChangeLanguageModel.PropertyNotChanged(GetType(), AiSettings.AiSettingsId,
             //   ("languageModelPath", AiSettings!.LanguageModelPath));
            return;
        }

        var changeLanguageModelCommand =
            new ChangeLanguageModelCommand
            {
                AiSettingsId = AiSettings.AiSettingsId.ToString(),
                LanguageModelPath = AiSettings.LanguageModelPath
            };

        await commandSender.Send(changeLanguageModelCommand);

        //tracer.AiSettings.ChangeLanguageModel.CommandSent(GetType(), AiSettings.AiSettingsId,
       //     changeLanguageModelCommand);

        _previousLanguageModelPath = AiSettings.LanguageModelPath;
    }
}