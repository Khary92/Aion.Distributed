using Avalonia.Controls;
using Client.Avalonia.Models.TimeTracking;

namespace Client.Avalonia.Views.Tracking;

public partial class TimeTrackingControl : UserControl
{
    public TimeTrackingControl(TimeTrackingViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}