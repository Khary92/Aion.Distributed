using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Documentation;

public class DocumentationViewModel(DocumentationModel documentationModel) : ReactiveObject, IInitializeAsync
{
    public DocumentationModel Model { get; } = documentationModel;

    public InitializationType Type => InitializationType.ViewModel;

    public Task InitializeAsync()
    {
        //TODO check why i can't remove this. This is weird...
        return Task.CompletedTask;
    }
}