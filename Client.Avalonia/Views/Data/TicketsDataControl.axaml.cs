using Avalonia.ReactiveUI;
using Client.Avalonia.Models.Data;
using Client.Avalonia.ViewModels.Data;

namespace Client.Avalonia.Views.Data;

public partial class TicketsDataControl : ReactiveUserControl<TicketsDataViewModel>
{
    public TicketsDataControl()
    {
        InitializeComponent();
    }
}