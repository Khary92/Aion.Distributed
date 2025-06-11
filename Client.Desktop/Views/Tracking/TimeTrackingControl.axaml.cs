using Avalonia.Controls;
using Client.Desktop.Models.TimeTracking;

namespace Client.Desktop.Views.Tracking;

public partial class TimeTrackingControl : UserControl
{
    public TimeTrackingControl(TimeTrackingViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}