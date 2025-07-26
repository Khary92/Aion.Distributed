using Avalonia.Controls;
using Client.Desktop.Presentation.Models.TimeTracking;
using ReactiveUI;

namespace Client.Desktop.Presentation.Views.Tracking;

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