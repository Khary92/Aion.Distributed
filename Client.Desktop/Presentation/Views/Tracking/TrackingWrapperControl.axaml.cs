using Avalonia.Controls;
using Client.Desktop.Presentation.Models.TimeTracking;

namespace Client.Desktop.Presentation.Views.Tracking;

public partial class TrackingWrapperControl : UserControl
{
    public TrackingWrapperControl()
    {
        InitializeComponent();
    }

    public TrackingWrapperControl(TimeTrackingViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}