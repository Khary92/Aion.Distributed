using Avalonia.Controls;
using Client.Desktop.Presentation.Models.Analysis;

namespace Client.Desktop.Presentation.Views.Analysis;

public partial class AnalysisControlWrapper : UserControl
{
    public AnalysisControlWrapper(AnalysisControlWrapperViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}