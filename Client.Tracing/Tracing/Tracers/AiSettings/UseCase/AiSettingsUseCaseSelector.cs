namespace Client.Tracing.Tracing.Tracers.AiSettings.UseCase;

public class AiSettingsUseCaseSelector(
    IChangeLanguageModelTraceCollector changeLanguageModelTraceCollector,
    IChangePromptTraceCollector changePromptTraceCollector) : IAiSettingsUseCaseSelector
{
    public IChangeLanguageModelTraceCollector ChangeLanguageModel => changeLanguageModelTraceCollector;
    public IChangePromptTraceCollector ChangePrompt => changePromptTraceCollector;
}