using Avalonia.Controls;
using Client.Avalonia.ViewModels.Documentation;
using ReactiveUI;

namespace Client.Avalonia.Views.Documentation;

public partial class DocumentationControl : UserControl, IViewFor<DocumentationViewModel>
{
    public DocumentationControl(DocumentationViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public DocumentationControl()
    {
        InitializeComponent();
    }

    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (DocumentationViewModel?)value;
    }

    public DocumentationViewModel? ViewModel { get; set; }
}