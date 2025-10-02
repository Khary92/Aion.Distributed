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
        //TODO This is required for the view to be constructed by DI services.
        return Task.CompletedTask;
    }
}