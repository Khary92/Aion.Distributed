using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.NotificationWrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.DTO;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using Proto.Command.AiSettings;
using Proto.Notifications.AiSettings;
using Proto.Requests.AiSettings;
using ReactiveUI;

namespace Client.Desktop.Models.Settings;

public class AiSettingsModel(
    ICommandSender commandSender,
    IRequestSender requestSender,
    IMessenger messenger,
    ITraceCollector tracer) : ReactiveObject

{
    private AiSettingsDto _aiSettings = new(Guid.NewGuid(), "settings not loaded", "settings not loaded");
    private string _previousLanguageModelPath = string.Empty;
    private string _previousPrompt = string.Empty;

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

        await commandSender.Send(new CreateAiSettingsCommandProto
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
        if (_previousPrompt == AiSettings.Prompt)
        {
            await tracer.AiSettings.ChangePrompt.PropertyNotChanged(GetType(), AiSettings.AiSettingsId,
                ("prompt", AiSettings.Prompt));
            return;
        }

        var changePromptCommand = new ChangePromptCommandProto
        {
            AiSettingsId = AiSettings.AiSettingsId.ToString(),
            Prompt = AiSettings.Prompt
        };

        await commandSender.Send(changePromptCommand);

        await tracer.AiSettings.ChangePrompt.CommandSent(GetType(), AiSettings.AiSettingsId, changePromptCommand);

        _previousPrompt = AiSettings.Prompt;
    }

    public async Task ChangeLanguageModelPath()
    {
        if (_previousLanguageModelPath == AiSettings.LanguageModelPath)
        {
            await tracer.AiSettings.ChangeLanguageModel.PropertyNotChanged(GetType(), AiSettings.AiSettingsId,
                ("languageModelPath", AiSettings!.LanguageModelPath));
            return;
        }

        var changeLanguageModelCommand =
            new ChangeLanguageModelCommandProto
            {
                AiSettingsId = AiSettings.AiSettingsId.ToString(),
                LanguageModelPath = AiSettings.LanguageModelPath
            };

        await commandSender.Send(changeLanguageModelCommand);

        await tracer.AiSettings.ChangeLanguageModel.CommandSent(GetType(), AiSettings.AiSettingsId,
            changeLanguageModelCommand);

        _previousLanguageModelPath = AiSettings.LanguageModelPath;
    }
}