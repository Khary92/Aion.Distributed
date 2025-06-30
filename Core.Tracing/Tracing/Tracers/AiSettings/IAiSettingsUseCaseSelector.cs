using Core.Server.Tracing.Tracing.Tracers.AiSettings.UseCase;

namespace Core.Server.Tracing.Tracing.Tracers.AiSettings;

public interface IAiSettingsUseCaseSelector
{
    IChangeLanguageModelTraceCollector ChangeLanguageModel { get; }
    IChangePromptTraceCollector ChangePrompt { get; }
}