using Avalonia.Controls;
using Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;
using ReactiveUI;

namespace Client.Desktop.Presentation.Views.Tracking.DynamicControl;

public partial class NoteListControl : UserControl, IViewFor<NoteStreamViewModel>
{
    public NoteListControl()
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