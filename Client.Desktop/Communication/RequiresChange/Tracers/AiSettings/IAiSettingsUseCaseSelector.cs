using Client.Desktop.Communication.RequiresChange.Tracers.AiSettings.UseCase;

namespace Client.Desktop.Communication.RequiresChange.Tracers.AiSettings;

public interface IAiSettingsUseCaseSelector
{
    IChangeLanguageModelTraceCollector ChangeLanguageModel { get; }
    IChangePromptTraceCollector ChangePrompt { get; }
}