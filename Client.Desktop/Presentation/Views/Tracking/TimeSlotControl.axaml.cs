using Avalonia.Controls;
using Client.Desktop.Presentation.Models.TimeTracking;
using ReactiveUI;

namespace Client.Desktop.Presentation.Views.Tracking;

public partial class TimeSlotControl : UserControl, IViewFor<TrackingSlotViewModel>
{
    public TimeSlotControl()
    {
        InitializeComponent();
    }

    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (TrackingSlotViewModel?)value;
    }

    public TrackingSlotViewModel? ViewModel { get; set; }
}