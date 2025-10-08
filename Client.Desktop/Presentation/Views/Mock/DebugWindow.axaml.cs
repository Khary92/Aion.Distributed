using Avalonia.Controls;

namespace Client.Desktop.Presentation.Views.Mock;

public partial class DebugWindow : Window
{
    public DebugWindow(Models.Mock.DebugWindow viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public DebugWindow()
    {
        InitializeComponent();
    }
}