using Avalonia.ReactiveUI;
using Client.Desktop.Presentation.Models.Mock;

namespace Client.Desktop.Presentation.Views.Mock;

public partial class ServerLogControl : ReactiveUserControl<ServerLogViewModel>
{
    public ServerLogControl()
    {
        InitializeComponent();
    }
}