using System.Reactive;
using System.Threading.Tasks;
using Client.Desktop.Tracing.Tracing.Tracers;
using ReactiveUI;

namespace Client.Desktop.Models.Settings;

public class AiSettingsViewModel : ReactiveObject
{
    private readonly ITraceCollector _tracer;

    public AiSettingsViewModel(ITraceCollector tracer, AiSettingsModel aiSettingsModel)
    {
        _tracer = tracer;
        ChangePromptCommand = ReactiveCommand.CreateFromTask(ChangePrompt);
        ChangeLanguageModelPathCommand = ReactiveCommand.CreateFromTask(ChangeLanguageModelPath);

        Model = aiSettingsModel;
        Model.Initialize().ConfigureAwait(false);
        Model.RegisterMessenger();
    }

    public AiSettingsModel Model { get; }
    public ReactiveCommand<Unit, Unit> ChangePromptCommand { get; }
    public ReactiveCommand<Unit, Unit> ChangeLanguageModelPathCommand { get; }


    private async Task ChangePrompt()
    {
        await _tracer.AiSettings.ChangePrompt.StartUseCase(GetType(), Model.AiSettings!.AiSettingsId,
            ("prompt", Model.AiSettings.Prompt));
        await Model.ChangePrompt();
    }

    private async Task ChangeLanguageModelPath()
    {
        await _tracer.AiSettings.ChangeLanguageModel.StartUseCase(GetType(), Model.AiSettings!.AiSettingsId,
            ("languageModelPath", Model.AiSettings!.LanguageModelPath));
        await Model.ChangeLanguageModelPath();
    }
}