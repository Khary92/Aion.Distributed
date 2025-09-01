using Avalonia.Controls;
using Client.Desktop.Presentation.Models.TimeTracking;

namespace Client.Desktop.Presentation.Views.Tracking;

public partial class TimeTrackingControl : UserControl
{
    public TimeTrackingControl()
    {
        InitializeComponent();
    }
    
    public TimeTrackingControl(TimeTrackingViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}