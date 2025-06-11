using Avalonia.Controls;
using Client.Avalonia.Models.TimeTracking.DynamicControls;
using ReactiveUI;

namespace Client.Avalonia.Views.Tracking.DynamicControl;

public partial class NotesStreamControl : UserControl, IViewFor<NoteStreamViewModel>
{
    public NotesStreamControl()
    {
        InitializeComponent();
    }

    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (NoteStreamViewModel?)value;
    }

    public NoteStreamViewModel? ViewModel { get; set; }
}