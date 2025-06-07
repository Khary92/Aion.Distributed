using Contract.Tracing.Tracers.AiSettings.UseCase;

namespace Contract.Tracing.Tracers.AiSettings;

public interface IAiSettingsUseCaseSelector
{
    IChangeLanguageModelTraceCollector ChangeLanguageModel { get; }
    IChangePromptTraceCollector ChangePrompt { get; }
}