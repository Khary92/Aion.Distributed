using Avalonia.Controls;
using Client.Avalonia.Models.TimeTracking.DynamicControls;
using ReactiveUI;

namespace Client.Avalonia.Views.Tracking.DynamicControl;

public partial class NoteControl : UserControl, IViewFor<NoteViewModel>
{
    public NoteControl()
    {
        InitializeComponent();
    }

    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (NoteViewModel?)value;
    }

    public NoteViewModel? ViewModel { get; set; }
}