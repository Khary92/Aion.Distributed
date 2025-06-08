using Avalonia.ReactiveUI;
using Client.Avalonia.ViewModels.Data;

namespace Client.Avalonia.Views.Data;

public partial class SprintsDataControl : ReactiveUserControl<SprintsDataViewModel>
{
    public SprintsDataControl()
    {
        InitializeComponent();
    }
}