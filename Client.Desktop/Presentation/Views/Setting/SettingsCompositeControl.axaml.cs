using Avalonia.ReactiveUI;
using Client.Desktop.Presentation.Models.Settings;

namespace Client.Desktop.Presentation.Views.Setting;

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