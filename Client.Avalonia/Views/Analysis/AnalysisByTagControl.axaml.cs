using Avalonia.Controls;
using Client.Avalonia.ViewModels.Analysis;
using ReactiveUI;

namespace Client.Avalonia.Views.Analysis;

public partial class AnalysisByTagControl : UserControl, IViewFor<AnalysisByTagViewModel>
{
    // Empty constructor required for view templating
    public AnalysisByTagControl()
    {
        InitializeComponent();
    }

    public AnalysisByTagControl(AnalysisByTagViewModel analysisByTagViewModel)
    {
        InitializeComponent();
        DataContext = analysisByTagViewModel;
    }

    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (AnalysisByTagViewModel)value!;
    }

    public AnalysisByTagViewModel? ViewModel { get; set; }
}