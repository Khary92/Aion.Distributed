using Avalonia.Controls;
using Client.Desktop.Models.Analysis;
using ReactiveUI;

namespace Client.Desktop.Views.Analysis;

public partial class AnalysisBySprintControl : UserControl, IViewFor<AnalysisBySprintViewModel>
{
    public AnalysisBySprintControl()
    {
        InitializeComponent();
    }

    public AnalysisBySprintControl(AnalysisBySprintViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (AnalysisBySprintViewModel)value!;
    }

    public AnalysisBySprintViewModel? ViewModel { get; set; }
}