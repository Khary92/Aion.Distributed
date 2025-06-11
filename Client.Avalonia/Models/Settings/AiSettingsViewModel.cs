using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;

namespace Client.Avalonia.Models.Settings;

public class AiSettingsViewModel : ReactiveObject
{

    public AiSettingsViewModel(AiSettingsModel aiSettingsModel)
    {
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
        //_tracer.AiSettings.ChangePrompt.StartUseCase(GetType(), Model.AiSettings!.AiSettingsId,
       //     ("prompt", Model.AiSettings.Prompt));
        await Model.ChangePrompt();
    }

    private async Task ChangeLanguageModelPath()
    {
        //_tracer.AiSettings.ChangeLanguageModel.StartUseCase(GetType(), Model.AiSettings!.AiSettingsId,
        //    ("languageModelPath", Model.AiSettings!.LanguageModelPath));
        await Model.ChangeLanguageModelPath();
    }
}