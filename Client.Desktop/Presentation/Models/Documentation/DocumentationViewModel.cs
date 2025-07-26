using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Client.Desktop.Services.Initializer;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Documentation;

public class DocumentationViewModel(DocumentationModel documentationModel) : ReactiveObject, IInitializeAsync
{
    public DocumentationModel Model { get; } = documentationModel;

    public InitializationType Type => InitializationType.ViewModel;

    public Task InitializeAsync()
    {
        this.WhenAnyValue(x => x.Model.SelectedTicket)
            .Where(ticket => ticket != null)
            .SelectMany(_ => Observable.FromAsync(Model.UpdateNotesForSelectedTicket))
            .Subscribe();

        return Task.CompletedTask;
    }
}