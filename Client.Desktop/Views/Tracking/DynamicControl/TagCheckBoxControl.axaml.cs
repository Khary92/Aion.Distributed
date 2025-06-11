using Avalonia.Controls;
using Client.Desktop.Models.TimeTracking.DynamicControls;
using ReactiveUI;

namespace Client.Desktop.Views.Tracking.DynamicControl;

public partial class TagCheckBoxControl : UserControl, IViewFor<TagCheckBoxViewModel>
{
    public TagCheckBoxControl()
    {
        InitializeComponent();
    }

    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (TagCheckBoxViewModel?)value;
    }

    public TagCheckBoxViewModel? ViewModel { get; set; }
}