using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Notifications.AiSettings;
using Client.Desktop.Communication.Requests;
using CommunityToolkit.Mvvm.Messaging;
using Contract.DTO;
using Proto.Command.AiSettings;
using Proto.Notifications.AiSettings;
using Proto.Requests.AiSettings;
using ReactiveUI;

namespace Client.Desktop.Models.Settings;

public class AiSettingsModel(
    ICommandSender commandSender,
    IRequestSender requestSender,
    IMessenger messenger) : ReactiveObject
{
    private string _previousLanguageModelPath = string.Empty;
    private string _previousPrompt = string.Empty;

    private AiSettingsDto _aiSettings = new(Guid.NewGuid(), "settings not loaded", "settings not loaded");
    
    public AiSettingsDto AiSettings
    {
        get => _aiSettings;
        private set => this.RaiseAndSetIfChanged(ref _aiSettings, value);
    }

    public async Task Initialize()
    {
        if (await requestSender.Send(new AiSettingExistsRequestProto()))
        {
            AiSettings = await requestSender.Send(new GetAiSettingsRequestProto());

            _previousPrompt = AiSettings.Prompt;
            _previousLanguageModelPath = AiSettings.LanguageModelPath;
            return;
        }

        await commandSender.Send(new CreateAiSettingsCommand
        {
            AiSettingsId = Guid.NewGuid().ToString(),
            LanguageModelPath = "Not set",
            Prompt = "not set"
        });
    }

    public void RegisterMessenger()
    {
        messenger.Register<NewAiSettingsMessage>(this, (_, m) =>
        {
            AiSettings = m.AiSettings;

            _previousPrompt = AiSettings.Prompt;
            _previousLanguageModelPath = AiSettings.LanguageModelPath;
        });

        messenger.Register<PromptChangedNotification>(this, (_, m) => { AiSettings.Apply(m); });

        messenger.Register<LanguageModelChangedNotification>(this, (_, m) => { AiSettings.Apply(m); });
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