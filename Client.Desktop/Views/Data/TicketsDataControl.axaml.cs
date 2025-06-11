using Avalonia.ReactiveUI;
using Client.Desktop.Models.Data;

namespace Client.Desktop.Views.Data;

public partial class TicketsDataControl : ReactiveUserControl<TicketsDataViewModel>
{
    public TicketsDataControl()
    {
        InitializeComponent();
    }
}