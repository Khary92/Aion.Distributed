using Client.Desktop.Tracing.Tracing.Tracers.AiSettings.UseCase;

namespace Client.Desktop.Tracing.Tracing.Tracers.AiSettings;

public interface IAiSettingsUseCaseSelector
{
    IChangeLanguageModelTraceCollector ChangeLanguageModel { get; }
    IChangePromptTraceCollector ChangePrompt { get; }
}