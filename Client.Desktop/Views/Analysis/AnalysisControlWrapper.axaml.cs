using Avalonia.Controls;
using Client.Desktop.Models.Analysis;

namespace Client.Desktop.Views.Analysis;

public partial class AnalysisControlWrapper : UserControl
{
    public AnalysisControlWrapper(AnalysisControlWrapperViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}