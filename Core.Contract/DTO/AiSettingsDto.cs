using Proto.Notifications.AiSettings;
using ReactiveUI;

namespace Contract.DTO;

public class AiSettingsDto : ReactiveObject
{
    private readonly Guid _aiSettingsId;
    private string _languageModelPath = string.Empty;
    private string _prompt = string.Empty;

    public AiSettingsDto(Guid aiSettingsId, string languageModelPath, string prompt)
    {
        AiSettingsId = aiSettingsId;
        LanguageModelPath = languageModelPath;
        Prompt = prompt;
    }

    public Guid AiSettingsId
    {
        get => _aiSettingsId;
        private init => this.RaiseAndSetIfChanged(ref _aiSettingsId, value);
    }

    public string Prompt
    {
        get => _prompt;
        set => this.RaiseAndSetIfChanged(ref _prompt, value);
    }

    public string LanguageModelPath
    {
        get => _languageModelPath;
        set => this.RaiseAndSetIfChanged(ref _languageModelPath, value);
    }

    public void Apply(PromptChangedNotification notification)
    {
        Prompt = notification.Prompt;
    }

    public void Apply(LanguageModelChangedNotification notification)
    {
        LanguageModelPath = notification.LanguageModelPath;
    }
}