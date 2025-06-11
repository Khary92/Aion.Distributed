using System.Reactive;
using System.Threading.Tasks;
using Contract.Tracing.Tracers;
using ReactiveUI;

namespace Client.Avalonia.ViewModels.Settings;

public class AiSettingsViewModel : ReactiveObject
{
    private readonly ITracingCollectorProvider _tracer;

    public AiSettingsViewModel(AiSettingsModel aiSettingsModel, ITracingCollectorProvider tracer)
    {
        _tracer = tracer;
        ChangePromptCommand = ReactiveCommand.CreateFromTask(ChangePrompt);
        ChangeLanguageModelPathCommand = ReactiveCommand.CreateFromTask(ChangeLanguageModelPath);

        Model = aiSettingsModel;
        Model.Initialize().ConfigureAwait(false);
    }

    public AiSettingsModel Model { get; }
    public ReactiveCommand<Unit, Unit> ChangePromptCommand { get; }
    public ReactiveCommand<Unit, Unit> ChangeLanguageModelPathCommand { get; }


    private async Task ChangePrompt()
    {
        _tracer.AiSettings.ChangePrompt.StartUseCase(GetType(), Model.AiSettings!.AiSettingsId,
            ("prompt", Model.AiSettings.Prompt));
        await Model.ChangePrompt();
    }

    private async Task ChangeLanguageModelPath()
    {
        _tracer.AiSettings.ChangeLanguageModel.StartUseCase(GetType(), Model.AiSettings!.AiSettingsId,
            ("languageModelPath", Model.AiSettings!.LanguageModelPath));
        await Model.ChangeLanguageModelPath();
    }
}