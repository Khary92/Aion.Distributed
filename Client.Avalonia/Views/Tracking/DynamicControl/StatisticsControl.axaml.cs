using Avalonia.Controls;
using Client.Avalonia.Models.TimeTracking.DynamicControls;
using ReactiveUI;

namespace Client.Avalonia.Views.Tracking.DynamicControl;

public partial class StatisticsControl : UserControl, IViewFor<StatisticsViewModel>
{
    public StatisticsControl()
    {
        InitializeComponent();
    }

    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (StatisticsViewModel?)value;
    }

    public StatisticsViewModel? ViewModel { get; set; }
}