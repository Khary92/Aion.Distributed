using System.Threading.Tasks;
using Contract.CQRS.Requests.AiSettings;
using Contract.DTO;
using Contract.Tracing.Tracers;
using MediatR;
using ReactiveUI;

namespace Client.Avalonia.ViewModels.Settings;

public class AiSettingsModel(IMediator mediator, ITracingCollectorProvider tracer) : ReactiveObject
{
    private string _previousLanguageModelPath = null!;

    private string _previousPrompt = null!;

    public AiSettingsDto? AiSettings { get; private set; }

    public async Task Initialize()
    {
        AiSettings = await mediator.Send(new GetAiSettingsRequest());
        _previousPrompt = AiSettings.Prompt;
        _previousLanguageModelPath = AiSettings.LanguageModelPath;
    }

    public async Task ChangePrompt()
    {
        if (_previousPrompt == AiSettings!.Prompt)
        {
            tracer.AiSettings.ChangePrompt.PropertyNotChanged(GetType(), AiSettings.AiSettingsId,
                ("propmpt", AiSettings!.Prompt));
            return;
        }

        var changePromptCommand = new ChangePromptCommand(AiSettings.AiSettingsId, AiSettings.Prompt);
        await mediator.Send(changePromptCommand);

        tracer.AiSettings.ChangePrompt.CommandSent(GetType(), AiSettings.AiSettingsId, changePromptCommand);

        _previousPrompt = AiSettings.Prompt;
    }

    public async Task ChangeLanguageModelPath()
    {
        if (_previousLanguageModelPath == AiSettings!.LanguageModelPath)
        {
            tracer.AiSettings.ChangeLanguageModel.PropertyNotChanged(GetType(), AiSettings.AiSettingsId,
                ("languageModelPath", AiSettings!.LanguageModelPath));
            return;
        }

        var changeLanguageModelCommand =
            new ChangeLanguageModelCommand(AiSettings.AiSettingsId, AiSettings.LanguageModelPath);
        await mediator.Send(changeLanguageModelCommand);

        tracer.AiSettings.ChangeLanguageModel.CommandSent(GetType(), AiSettings.AiSettingsId,
            changeLanguageModelCommand);

        _previousLanguageModelPath = AiSettings.LanguageModelPath;
    }
}