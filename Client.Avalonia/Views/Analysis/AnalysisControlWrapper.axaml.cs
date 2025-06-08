using Avalonia.Controls;
using Client.Avalonia.ViewModels.Analysis;

namespace Client.Avalonia.Views.Analysis;

public partial class AnalysisControlWrapper : UserControl
{
    public AnalysisControlWrapper(AnalysisControlWrapperViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}