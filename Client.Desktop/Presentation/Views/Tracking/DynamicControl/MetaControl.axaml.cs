using Avalonia.Controls;
using Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;
using ReactiveUI;

namespace Client.Desktop.Presentation.Views.Tracking.DynamicControl;

public partial class MetaControl : UserControl, IViewFor<StatisticsViewModel>
{
    public MetaControl()
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