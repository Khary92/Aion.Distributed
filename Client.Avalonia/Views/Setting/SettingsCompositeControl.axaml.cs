using Avalonia.ReactiveUI;
using Client.Avalonia.ViewModels.Settings;

namespace Client.Avalonia.Views.Setting;

public partial class SettingsCompositeControl : ReactiveUserControl<SettingsCompositeViewModel>
{
    public SettingsCompositeControl()
    {
        InitializeComponent();
    }

    public SettingsCompositeControl(SettingsCompositeViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}