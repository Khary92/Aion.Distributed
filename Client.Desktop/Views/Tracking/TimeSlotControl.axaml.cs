using Avalonia.Controls;
using Client.Desktop.Models.TimeTracking;
using ReactiveUI;

namespace Client.Desktop.Views.Tracking;

public partial class TimeSlotControl : UserControl, IViewFor<TimeSlotViewModel>
{
    public TimeSlotControl()
    {
        InitializeComponent();
    }

    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (TimeSlotViewModel?)value;
    }

    public TimeSlotViewModel? ViewModel { get; set; }
}