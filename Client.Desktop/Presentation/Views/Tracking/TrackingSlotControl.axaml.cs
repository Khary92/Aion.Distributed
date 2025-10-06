using Avalonia.ReactiveUI;
using Client.Desktop.Presentation.Models.TimeTracking;

namespace Client.Desktop.Presentation.Views.Tracking;

public partial class TrackingSlotControl : ReactiveUserControl<TrackingSlotViewModel>
{
    public TrackingSlotControl()
    {
        InitializeComponent();
    }
}