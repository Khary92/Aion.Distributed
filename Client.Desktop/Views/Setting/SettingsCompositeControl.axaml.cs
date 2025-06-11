using Avalonia.ReactiveUI;
using Client.Desktop.Models.Settings;

namespace Client.Desktop.Views.Setting;

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