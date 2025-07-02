using Client.Tracing.Tracing.Tracers.AiSettings.UseCase;

namespace Client.Tracing.Tracing.Tracers.AiSettings;

public interface IAiSettingsUseCaseSelector
{
    IChangeLanguageModelTraceCollector ChangeLanguageModel { get; }
    IChangePromptTraceCollector ChangePrompt { get; }
}