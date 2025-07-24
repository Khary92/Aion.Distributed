using System;
using System.Reactive.Linq;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Communication.RequiresChange;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Models.Documentation;

public class DocumentationViewModel : ReactiveObject
{
    private readonly ILanguageModelApi _languageModelApi;
    private readonly IRequestSender _requestSender;
    private string _inputText = string.Empty;

    private string _responseText = string.Empty;

    public DocumentationViewModel(IRequestSender requestSender, DocumentationModel documentationModel,
        ILanguageModelApi languageModelApi)
    {
        _requestSender = requestSender;
        _languageModelApi = languageModelApi;

        Model = documentationModel;

        Model.Initialize().ConfigureAwait(false);
        Model.RegisterMessenger();

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
}