using Avalonia.Controls;
using Client.Desktop.Models.Documentation;
using ReactiveUI;

namespace Client.Desktop.Views.Documentation;

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