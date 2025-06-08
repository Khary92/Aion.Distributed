using Avalonia.ReactiveUI;
using Client.Avalonia.ViewModels.Tracing;

namespace Client.Avalonia.Views.Tracing;

public partial class TracingControl : ReactiveUserControl<TracingViewModel>
{
    public TracingControl()
    {
        InitializeComponent();
    }

    public TracingControl(TracingViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}