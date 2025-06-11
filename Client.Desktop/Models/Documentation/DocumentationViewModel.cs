using System;
using System.Reactive.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Contract.CQRS.Requests.AiSettings;
using Contract.LanguageModel;
using MediatR;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Models.Documentation;

public class DocumentationViewModel : ReactiveObject
{
    private readonly ILanguageModelApi _languageModelApi;
    private readonly IMediator _mediator;
    private string _inputText = string.Empty;

    private string _responseText = string.Empty;

    public DocumentationViewModel(IMediator mediator, DocumentationModel documentationModel,
        ILanguageModelApi languageModelApi)
    {
        _mediator = mediator;
        _languageModelApi = languageModelApi;
        _languageModelApi.OnResponseReceived += WriteGptResponse;

        Model = documentationModel;

        Model.Initialize().ConfigureAwait(false);
        Model.RegisterMessenger();

        LoadModelCommand = ReactiveCommand.CreateFromTask(ReloadGptModel);
        CancelPromptCommand = ReactiveCommand.Create(CancelLanguageModelPrompt);
        SetPromptCommand = ReactiveCommand.CreateFromTask(SetPreparedPrompt);
        SendRequestCommand = ReactiveCommand.Create(SendLanguageModelRequest);

        this.WhenAnyValue(x => x.Model.SelectedTicket)
            .Where(ticket => ticket != null)
            .SelectMany(_ => Observable.FromAsync(Model.UpdateNotesForSelectedTicket))
            .Subscribe();
    }

    public DocumentationModel Model { get; }

    public string ResponseText
    {
        get => _responseText;
        set => this.RaiseAndSetIfChanged(ref _responseText, value);
    }

    public string InputText
    {
        get => _inputText;
        set => this.RaiseAndSetIfChanged(ref _inputText, value);
    }

    public ReactiveCommand<Unit, Unit> LoadModelCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelPromptCommand { get; }
    public ReactiveCommand<Unit, Unit> SendRequestCommand { get; }
    public ReactiveCommand<Unit, Unit> SetPromptCommand { get; }

    private async Task SetPreparedPrompt()
    {
        InputText = string.Empty;
        var aiSettingsDto = await _mediator.Send(new GetAiSettingsRequest());

        var inputBuilder = new StringBuilder();

        inputBuilder.AppendLine(aiSettingsDto.Prompt);
        inputBuilder.AppendLine();


        foreach (var viewModel in Model.SelectedNotes)
        {
            var simpleObject = new
            {
                viewModel.Note.NoteTypeId,
                viewModel.Note.Text
            };

            var json = JsonSerializer.Serialize(simpleObject);

            inputBuilder.AppendLine(json);
        }

        InputText = inputBuilder.ToString();
    }

    private async Task ReloadGptModel()
    {
        await _languageModelApi.ReloadModel();
    }

    private void CancelLanguageModelPrompt()
    {
        _languageModelApi.CancelRequest();
    }

    private void SendLanguageModelRequest()
    {
        ResponseText = string.Empty;
        _languageModelApi.StartRequest(InputText);
    }

    private void WriteGptResponse(object? sender, string e)
    {
        ResponseText += e;
    }
}